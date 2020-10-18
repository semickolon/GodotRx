const signalData = require('./signal_data.json')

const OBJ_VAR_NAME = 'obj'
const SIGNAL_EXT_CLS_NAME = 'SignalExtensions'
const STATIC_EXT_POSTFIX = 'Signals'
const INDENT = ' '.repeat(2)

const CLASS_STATIC = [
  'AudioServer',
  'ARVRServer',
  'CameraServer',
  'Input',
  'VisualServer',
  'VisualScriptEditor'
]

const TYPE_MAP = {
  0: "object",
  1: "bool",
  2: "int",
  3: "float",
  4: "string",
  5: "Vector2",
  6: "Rect2",
  7: "Vector3",
  8: "Transform2D",
  9: "Plane",
  10: "Quat",
  11: "AABB",
  12: "Basis",
  13: "Transform",
  14: "Color",
  15: "NodePath",
  16: "RID",
  17: "Godot.Object",
  18: "Godot.Collections.Dictionary",
  19: "Godot.Collections.Array",
  20: "byte[]",
  21: "int[]",
  22: "float[]",
  23: "string[]",
  24: "Vector2[]",
  25: "Vector3[]",
  26: "Color[]"
}

function trimStart(str, pre) {
  if (str.startsWith(pre))
    return str.substr(pre.length)
  return str
}

function snakeToPascal(str) {
  return str
    .split('_')
    .filter(s => !!s)
    .map(s => s[0].toUpperCase() + s.substr(1))
    .join('')
}

function stringifyType({ type: id, cls }) {
  if (id === 17 && cls.length > 0) {
    return `Godot.${cls}`
  }
  return TYPE_MAP[id]
}

function funcDecl(className, signalName, args, static) {
  let generics = ""
  let callGenerics = ""

  if (args.length === 1) {
    generics = stringifyType(args[0])
    callGenerics = `<${generics}>`
  } else if (args.length > 1) {
    generics = args
      .map(arg => ({
        type: stringifyType(arg),
        name: snakeToPascal(arg.name)
      }))
      .map(arg => `${arg.type} ${arg.name}`)
      .join(', ')

    callGenerics = args
      .map(arg => stringifyType(arg))
      .join(', ')
    
    generics = `(${generics})`
    callGenerics = `<${callGenerics}>`
  }

  let returnType = `IObservable<${generics || 'Unit'}>`
  let methodName = `On${trimStart(snakeToPascal(signalName), 'On')}`
  let params = static ? "" : `this Godot.${className} ${OBJ_VAR_NAME}`;
  let instance = static ? `${className}.Singleton` : OBJ_VAR_NAME;
  
  return `
public static ${returnType} ${methodName}(${params})
  => ${instance}.ObserveSignal${callGenerics}("${signalName}");
`.trim();
}

function generateClassReg() {
  let classReg = {}
  let maxArgs = 0

  for (let className in signalData) {
    let genClassName = trimStart(className, '_')
    let static = CLASS_STATIC.includes(genClassName)

    let classRegKey = static
      ? `${genClassName}${STATIC_EXT_POSTFIX}`
      : SIGNAL_EXT_CLS_NAME
    
    let signals = signalData[className]

    for (let signalName in signals) {
      let args = signals[signalName]
      maxArgs = Math.max(maxArgs, args.length)
  
      let decl = funcDecl(genClassName, signalName, args, static)
      
      if (!(classRegKey in classReg)) {
        classReg[classRegKey] = []
      }

      classReg[classRegKey].push(decl)
    }
  }

  // console.log(`Max args: ${maxArgs}`);
  return classReg;
}

function generateCode() {
  let classReg = generateClassReg();

  return `
using Godot;
using System;
using System.Reactive;

namespace GodotRx {
${
  Object.keys(classReg).map(classRegKey => `
public static class ${classRegKey} {
${
  classReg[classRegKey]
    .join('\n\n')
    .indent()
}
}
`.trim().indent())
  .join('\n\n')
}
}
`.trim()
}

String.prototype.indent = function() {
  return this.split('\n')
    .map(s => s.trim().length > 0 ? INDENT + s : '')
    .join('\n')
}

console.log(generateCode())
