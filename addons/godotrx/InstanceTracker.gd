extends Reference

var id = get_instance_id()
var GodotRx: Node

func _init(rx: Node):
	GodotRx = rx

func _notification(what):
	if what == NOTIFICATION_PREDELETE:
		GodotRx.on_instance_tracker_predelete(id)
