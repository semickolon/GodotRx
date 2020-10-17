tool
extends EditorPlugin

func _enter_tree():
	add_autoload_singleton("GodotRx", "res://addons/godotrx/Internal/Singleton.cs")
	add_custom_type("SignalDataGen", "Node", preload("SignalDataGen.gd"), null)

func _exit_tree():
	remove_autoload_singleton("GodotRx")
	remove_custom_type("SignalDataGen")
