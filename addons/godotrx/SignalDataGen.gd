extends Node

export var save_path = "res://addons/godotrx/signal_data.json"

func _ready():
	save_signal_data()

func save_signal_data():
	var db = {}

	for cls in ClassDB.get_class_list():
		var cls_signal_data: Dictionary = get_class_signal_data(cls)

		if not cls_signal_data.empty():
			db[cls] = cls_signal_data

	var json = JSON.print(db)
	var f = File.new()

	f.open(save_path, File.WRITE)
	f.store_string(json)
	f.close()

	print("Saved signal data to ", save_path)

func get_class_signal_data(cls) -> Dictionary:
	var out = {}
	var signals = ClassDB.class_get_signal_list(cls, true)
	
	for signal_info in signals:
		var args = []

		for arg in signal_info.args:
			args.append({ "name": arg.name, "type": arg.type, "cls": arg["class_name"] })
		
		out[signal_info.name] = args
	
	return out
