extends Node

const InstanceTracker = preload("InstanceTracker.gd")
const TRK_META = "__inst_trk__"

signal instance_tracker_freed(id)

func inject_instance_tracker(obj: Object) -> int:
	var tracker: InstanceTracker

	if obj.has_meta(TRK_META):
		tracker = obj.get_meta(TRK_META)
	else:
		tracker = InstanceTracker.new(self)
		obj.set_meta(TRK_META, tracker)
	
	return tracker.id

func on_instance_tracker_predelete(id):
	emit_signal("instance_tracker_freed", id)
