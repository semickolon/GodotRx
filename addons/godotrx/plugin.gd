tool
extends EditorPlugin

func _enter_tree():
	add_custom_type("SignalDataGen", "Node", preload("SignalDataGen.gd"), null)

func _exit_tree():
	remove_custom_type("SignalDataGen")
