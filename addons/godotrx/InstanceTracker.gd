extends Reference

var id = get_instance_id()
var GodotRx: Object

func _init(rx: Object):
	GodotRx = rx

func _notification(what):
	if what == NOTIFICATION_PREDELETE:
		GodotRx.on_instance_tracker_predelete(id)
