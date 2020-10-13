using Godot;
using System;
using System.Reactive;

using Object = Godot.Object;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;

namespace GodotRx
{
  public static class SignalExtensions
  {
    public static IObservable<Unit> OnScriptChanged(this Object obj)
      => obj.ObserveSignal("script_changed");
    public static IObservable<Unit> OnChanged(this Resource obj)
      => obj.ObserveSignal("changed");
    public static IObservable<Unit> OnConnectedToServer(this MultiplayerAPI obj)
      => obj.ObserveSignal("connected_to_server");
    public static IObservable<Unit> OnConnectionFailed(this MultiplayerAPI obj)
      => obj.ObserveSignal("connection_failed");
    public static IObservable<(int Id, byte[] Packet)> OnNetworkPeerPacket(this MultiplayerAPI obj)
      => obj.ObserveSignal<int, byte[]>("network_peer_packet");
    public static IObservable<int> OnNetworkPeerDisconnected(this MultiplayerAPI obj)
      => obj.ObserveSignal<int>("network_peer_disconnected");
    public static IObservable<int> OnNetworkPeerConnected(this MultiplayerAPI obj)
      => obj.ObserveSignal<int>("network_peer_connected");
    public static IObservable<Unit> OnServerDisconnected(this MultiplayerAPI obj)
      => obj.ObserveSignal("server_disconnected");
    public static IObservable<Unit> OnConnectionFailed(this NetworkedMultiplayerPeer obj)
      => obj.ObserveSignal("connection_failed");
    public static IObservable<Unit> OnConnectionSucceeded(this NetworkedMultiplayerPeer obj)
      => obj.ObserveSignal("connection_succeeded");
    public static IObservable<int> OnPeerDisconnected(this NetworkedMultiplayerPeer obj)
      => obj.ObserveSignal<int>("peer_disconnected");
    public static IObservable<int> OnPeerConnected(this NetworkedMultiplayerPeer obj)
      => obj.ObserveSignal<int>("peer_connected");
    public static IObservable<Unit> OnServerDisconnected(this NetworkedMultiplayerPeer obj)
      => obj.ObserveSignal("server_disconnected");
    public static IObservable<(string Permission, bool Granted)> OnOnRequestPermissionsResult(this MainLoop obj)
      => obj.ObserveSignal<string, bool>("on_request_permissions_result");
    public static IObservable<Unit> OnVersionChanged(this UndoRedo obj)
      => obj.ObserveSignal("version_changed");
    public static IObservable<Unit> OnRenamed(this Node obj)
      => obj.ObserveSignal("renamed");
    public static IObservable<Unit> OnReady(this Node obj)
      => obj.ObserveSignal("ready");
    public static IObservable<Unit> OnTreeEntered(this Node obj)
      => obj.ObserveSignal("tree_entered");
    public static IObservable<Unit> OnTreeExiting(this Node obj)
      => obj.ObserveSignal("tree_exiting");
    public static IObservable<Unit> OnTreeExited(this Node obj)
      => obj.ObserveSignal("tree_exited");
    public static IObservable<Unit> OnSizeChanged(this Viewport obj)
      => obj.ObserveSignal("size_changed");
    public static IObservable<Godot.Control> OnGuiFocusChanged(this Viewport obj)
      => obj.ObserveSignal<Godot.Control>("gui_focus_changed");
    public static IObservable<Unit> OnMouseExited(this Control obj)
      => obj.ObserveSignal("mouse_exited");
    public static IObservable<Godot.InputEvent> OnGuiInput(this Control obj)
      => obj.ObserveSignal<Godot.InputEvent>("gui_input");
    public static IObservable<Unit> OnModalClosed(this Control obj)
      => obj.ObserveSignal("modal_closed");
    public static IObservable<Unit> OnFocusEntered(this Control obj)
      => obj.ObserveSignal("focus_entered");
    public static IObservable<Unit> OnResized(this Control obj)
      => obj.ObserveSignal("resized");
    public static IObservable<Unit> OnMinimumSizeChanged(this Control obj)
      => obj.ObserveSignal("minimum_size_changed");
    public static IObservable<Unit> OnMouseEntered(this Control obj)
      => obj.ObserveSignal("mouse_entered");
    public static IObservable<Unit> OnSizeFlagsChanged(this Control obj)
      => obj.ObserveSignal("size_flags_changed");
    public static IObservable<Unit> OnFocusExited(this Control obj)
      => obj.ObserveSignal("focus_exited");
    public static IObservable<(int Result, int ResponseCode, string[] Headers, byte[] Body)> OnRequestCompleted(this HTTPRequest obj)
      => obj.ObserveSignal<int, int, string[], byte[]>("request_completed");
    public static IObservable<Unit> OnTimeout(this Timer obj)
      => obj.ObserveSignal("timeout");
    public static IObservable<Unit> OnItemRectChanged(this CanvasItem obj)
      => obj.ObserveSignal("item_rect_changed");
    public static IObservable<Unit> OnDraw(this CanvasItem obj)
      => obj.ObserveSignal("draw");
    public static IObservable<Unit> OnVisibilityChanged(this CanvasItem obj)
      => obj.ObserveSignal("visibility_changed");
    public static IObservable<Unit> OnHide(this CanvasItem obj)
      => obj.ObserveSignal("hide");
    public static IObservable<Unit> OnButtonDown(this BaseButton obj)
      => obj.ObserveSignal("button_down");
    public static IObservable<bool> OnToggled(this BaseButton obj)
      => obj.ObserveSignal<bool>("toggled");
    public static IObservable<Unit> OnPressed(this BaseButton obj)
      => obj.ObserveSignal("pressed");
    public static IObservable<Unit> OnButtonUp(this BaseButton obj)
      => obj.ObserveSignal("button_up");
    public static IObservable<float> OnValueChanged(this Range obj)
      => obj.ObserveSignal<float>("value_changed");
    public static IObservable<Unit> OnChanged(this Range obj)
      => obj.ObserveSignal("changed");
    public static IObservable<Unit> OnScrolling(this ScrollBar obj)
      => obj.ObserveSignal("scrolling");
    public static IObservable<Unit> OnPopupHide(this Popup obj)
      => obj.ObserveSignal("popup_hide");
    public static IObservable<Unit> OnAboutToShow(this Popup obj)
      => obj.ObserveSignal("about_to_show");
    public static IObservable<Unit> OnAboutToShow(this MenuButton obj)
      => obj.ObserveSignal("about_to_show");
    public static IObservable<Unit> OnTextureChanged(this NinePatchRect obj)
      => obj.ObserveSignal("texture_changed");
    public static IObservable<Unit> OnPrePopupPressed(this TabContainer obj)
      => obj.ObserveSignal("pre_popup_pressed");
    public static IObservable<int> OnTabSelected(this TabContainer obj)
      => obj.ObserveSignal<int>("tab_selected");
    public static IObservable<int> OnTabChanged(this TabContainer obj)
      => obj.ObserveSignal<int>("tab_changed");
    public static IObservable<Unit> OnSortChildren(this Container obj)
      => obj.ObserveSignal("sort_children");
    public static IObservable<int> OnTabClose(this Tabs obj)
      => obj.ObserveSignal<int>("tab_close");
    public static IObservable<int> OnTabClicked(this Tabs obj)
      => obj.ObserveSignal<int>("tab_clicked");
    public static IObservable<int> OnRepositionActiveTabRequest(this Tabs obj)
      => obj.ObserveSignal<int>("reposition_active_tab_request");
    public static IObservable<int> OnRightButtonPressed(this Tabs obj)
      => obj.ObserveSignal<int>("right_button_pressed");
    public static IObservable<int> OnTabChanged(this Tabs obj)
      => obj.ObserveSignal<int>("tab_changed");
    public static IObservable<int> OnTabHover(this Tabs obj)
      => obj.ObserveSignal<int>("tab_hover");
    public static IObservable<Unit> OnScrollStarted(this ScrollContainer obj)
      => obj.ObserveSignal("scroll_started");
    public static IObservable<Unit> OnScrollEnded(this ScrollContainer obj)
      => obj.ObserveSignal("scroll_ended");
    public static IObservable<int> OnItemActivated(this ItemList obj)
      => obj.ObserveSignal<int>("item_activated");
    public static IObservable<(int Index, bool Selected)> OnMultiSelected(this ItemList obj)
      => obj.ObserveSignal<int, bool>("multi_selected");
    public static IObservable<Unit> OnNothingSelected(this ItemList obj)
      => obj.ObserveSignal("nothing_selected");
    public static IObservable<Vector2> OnRmbClicked(this ItemList obj)
      => obj.ObserveSignal<Vector2>("rmb_clicked");
    public static IObservable<(int Index, Vector2 AtPosition)> OnItemRmbSelected(this ItemList obj)
      => obj.ObserveSignal<int, Vector2>("item_rmb_selected");
    public static IObservable<int> OnItemSelected(this ItemList obj)
      => obj.ObserveSignal<int>("item_selected");
    public static IObservable<string> OnTextEntered(this LineEdit obj)
      => obj.ObserveSignal<string>("text_entered");
    public static IObservable<string> OnTextChanged(this LineEdit obj)
      => obj.ObserveSignal<string>("text_changed");
    public static IObservable<Unit> OnTextChangeRejected(this LineEdit obj)
      => obj.ObserveSignal("text_change_rejected");
    public static IObservable<Unit> OnFinished(this VideoPlayer obj)
      => obj.ObserveSignal("finished");
    public static IObservable<string[]> OnFilesSelected(this FileDialog obj)
      => obj.ObserveSignal<string[]>("files_selected");
    public static IObservable<string> OnDirSelected(this FileDialog obj)
      => obj.ObserveSignal<string>("dir_selected");
    public static IObservable<string> OnFileSelected(this FileDialog obj)
      => obj.ObserveSignal<string>("file_selected");
    public static IObservable<Unit> OnConfirmed(this AcceptDialog obj)
      => obj.ObserveSignal("confirmed");
    public static IObservable<string> OnCustomAction(this AcceptDialog obj)
      => obj.ObserveSignal<string>("custom_action");
    public static IObservable<int> OnIndexPressed(this PopupMenu obj)
      => obj.ObserveSignal<int>("index_pressed");
    public static IObservable<int> OnIdFocused(this PopupMenu obj)
      => obj.ObserveSignal<int>("id_focused");
    public static IObservable<int> OnIdPressed(this PopupMenu obj)
      => obj.ObserveSignal<int>("id_pressed");
    public static IObservable<Unit> OnItemActivated(this Tree obj)
      => obj.ObserveSignal("item_activated");
    public static IObservable<(Godot.TreeItem Item, int Column, bool Selected)> OnMultiSelected(this Tree obj)
      => obj.ObserveSignal<Godot.TreeItem, int, bool>("multi_selected");
    public static IObservable<int> OnColumnTitlePressed(this Tree obj)
      => obj.ObserveSignal<int>("column_title_pressed");
    public static IObservable<bool> OnCustomPopupEdited(this Tree obj)
      => obj.ObserveSignal<bool>("custom_popup_edited");
    public static IObservable<Godot.TreeItem> OnItemCollapsed(this Tree obj)
      => obj.ObserveSignal<Godot.TreeItem>("item_collapsed");
    public static IObservable<Unit> OnItemRmbEdited(this Tree obj)
      => obj.ObserveSignal("item_rmb_edited");
    public static IObservable<Unit> OnItemEdited(this Tree obj)
      => obj.ObserveSignal("item_edited");
    public static IObservable<Vector2> OnEmptyTreeRmbSelected(this Tree obj)
      => obj.ObserveSignal<Vector2>("empty_tree_rmb_selected");
    public static IObservable<Unit> OnNothingSelected(this Tree obj)
      => obj.ObserveSignal("nothing_selected");
    public static IObservable<Unit> OnItemDoubleClicked(this Tree obj)
      => obj.ObserveSignal("item_double_clicked");
    public static IObservable<Vector2> OnEmptyRmb(this Tree obj)
      => obj.ObserveSignal<Vector2>("empty_rmb");
    public static IObservable<Vector2> OnItemRmbSelected(this Tree obj)
      => obj.ObserveSignal<Vector2>("item_rmb_selected");
    public static IObservable<Unit> OnItemSelected(this Tree obj)
      => obj.ObserveSignal("item_selected");
    public static IObservable<Unit> OnCellSelected(this Tree obj)
      => obj.ObserveSignal("cell_selected");
    public static IObservable<(Godot.TreeItem Item, int Column, int Id)> OnButtonPressed(this Tree obj)
      => obj.ObserveSignal<Godot.TreeItem, int, int>("button_pressed");
    public static IObservable<Unit> OnItemCustomButtonPressed(this Tree obj)
      => obj.ObserveSignal("item_custom_button_pressed");
    public static IObservable<int> OnBreakpointToggled(this TextEdit obj)
      => obj.ObserveSignal<int>("breakpoint_toggled");
    public static IObservable<Unit> OnTextChanged(this TextEdit obj)
      => obj.ObserveSignal("text_changed");
    public static IObservable<(string Symbol, int Row, int Column)> OnSymbolLookup(this TextEdit obj)
      => obj.ObserveSignal<string, int, int>("symbol_lookup");
    public static IObservable<Unit> OnCursorChanged(this TextEdit obj)
      => obj.ObserveSignal("cursor_changed");
    public static IObservable<(int Row, string Info)> OnInfoClicked(this TextEdit obj)
      => obj.ObserveSignal<int, string>("info_clicked");
    public static IObservable<Unit> OnRequestCompletion(this TextEdit obj)
      => obj.ObserveSignal("request_completion");
    public static IObservable<int> OnItemFocused(this OptionButton obj)
      => obj.ObserveSignal<int>("item_focused");
    public static IObservable<int> OnItemSelected(this OptionButton obj)
      => obj.ObserveSignal<int>("item_selected");
    public static IObservable<Color> OnPresetRemoved(this ColorPicker obj)
      => obj.ObserveSignal<Color>("preset_removed");
    public static IObservable<Color> OnPresetAdded(this ColorPicker obj)
      => obj.ObserveSignal<Color>("preset_added");
    public static IObservable<Color> OnColorChanged(this ColorPicker obj)
      => obj.ObserveSignal<Color>("color_changed");
    public static IObservable<object> OnMetaClicked(this RichTextLabel obj)
      => obj.ObserveSignal<object>("meta_clicked");
    public static IObservable<object> OnMetaHoverStarted(this RichTextLabel obj)
      => obj.ObserveSignal<object>("meta_hover_started");
    public static IObservable<object> OnMetaHoverEnded(this RichTextLabel obj)
      => obj.ObserveSignal<object>("meta_hover_ended");
    public static IObservable<Unit> OnPickerCreated(this ColorPickerButton obj)
      => obj.ObserveSignal("picker_created");
    public static IObservable<Unit> OnPopupClosed(this ColorPickerButton obj)
      => obj.ObserveSignal("popup_closed");
    public static IObservable<Color> OnColorChanged(this ColorPickerButton obj)
      => obj.ObserveSignal<Color>("color_changed");
    public static IObservable<Unit> OnRaiseRequest(this GraphNode obj)
      => obj.ObserveSignal("raise_request");
    public static IObservable<Unit> OnCloseRequest(this GraphNode obj)
      => obj.ObserveSignal("close_request");
    public static IObservable<(Vector2 From, Vector2 To)> OnDragged(this GraphNode obj)
      => obj.ObserveSignal<Vector2, Vector2>("dragged");
    public static IObservable<Unit> OnOffsetChanged(this GraphNode obj)
      => obj.ObserveSignal("offset_changed");
    public static IObservable<Vector2> OnResizeRequest(this GraphNode obj)
      => obj.ObserveSignal<Vector2>("resize_request");
    public static IObservable<int> OnDragged(this SplitContainer obj)
      => obj.ObserveSignal<int>("dragged");
    public static IObservable<Unit> OnDeleteNodesRequest(this GraphEdit obj)
      => obj.ObserveSignal("delete_nodes_request");
    public static IObservable<Unit> OnCopyNodesRequest(this GraphEdit obj)
      => obj.ObserveSignal("copy_nodes_request");
    public static IObservable<Unit> OnDuplicateNodesRequest(this GraphEdit obj)
      => obj.ObserveSignal("duplicate_nodes_request");
    public static IObservable<Vector2> OnPopupRequest(this GraphEdit obj)
      => obj.ObserveSignal<Vector2>("popup_request");
    public static IObservable<Unit> OnPasteNodesRequest(this GraphEdit obj)
      => obj.ObserveSignal("paste_nodes_request");
    public static IObservable<Vector2> OnScrollOffsetChanged(this GraphEdit obj)
      => obj.ObserveSignal<Vector2>("scroll_offset_changed");
    public static IObservable<Godot.Node> OnNodeSelected(this GraphEdit obj)
      => obj.ObserveSignal<Godot.Node>("node_selected");
    public static IObservable<Unit> OnBeginNodeMove(this GraphEdit obj)
      => obj.ObserveSignal("_begin_node_move");
    public static IObservable<(string From, int FromSlot, Vector2 ReleasePosition)> OnConnectionToEmpty(this GraphEdit obj)
      => obj.ObserveSignal<string, int, Vector2>("connection_to_empty");
    public static IObservable<(string From, int FromSlot, string To, int ToSlot)> OnDisconnectionRequest(this GraphEdit obj)
      => obj.ObserveSignal<string, int, string, int>("disconnection_request");
    public static IObservable<(string From, int FromSlot, string To, int ToSlot)> OnConnectionRequest(this GraphEdit obj)
      => obj.ObserveSignal<string, int, string, int>("connection_request");
    public static IObservable<Unit> OnEndNodeMove(this GraphEdit obj)
      => obj.ObserveSignal("_end_node_move");
    public static IObservable<(string To, int ToSlot, Vector2 ReleasePosition)> OnConnectionFromEmpty(this GraphEdit obj)
      => obj.ObserveSignal<string, int, Vector2>("connection_from_empty");
    public static IObservable<Godot.Node> OnNodeUnselected(this GraphEdit obj)
      => obj.ObserveSignal<Godot.Node>("node_unselected");
    public static IObservable<Unit> OnVisibilityChanged(this Spatial obj)
      => obj.ObserveSignal("visibility_changed");
    public static IObservable<Unit> OnCachesCleared(this AnimationPlayer obj)
      => obj.ObserveSignal("caches_cleared");
    public static IObservable<string> OnAnimationStarted(this AnimationPlayer obj)
      => obj.ObserveSignal<string>("animation_started");
    public static IObservable<(string OldName, string NewName)> OnAnimationChanged(this AnimationPlayer obj)
      => obj.ObserveSignal<string, string>("animation_changed");
    public static IObservable<string> OnAnimationFinished(this AnimationPlayer obj)
      => obj.ObserveSignal<string>("animation_finished");
    public static IObservable<(Object Object, NodePath Key, float Elapsed, Object Value)> OnTweenStep(this Tween obj)
      => obj.ObserveSignal<Object, NodePath, float, Object>("tween_step");
    public static IObservable<Unit> OnTweenAllCompleted(this Tween obj)
      => obj.ObserveSignal("tween_all_completed");
    public static IObservable<(Object Object, NodePath Key)> OnTweenCompleted(this Tween obj)
      => obj.ObserveSignal<Object, NodePath>("tween_completed");
    public static IObservable<(Object Object, NodePath Key)> OnTweenStarted(this Tween obj)
      => obj.ObserveSignal<Object, NodePath>("tween_started");
    public static IObservable<Unit> OnRemovedFromGraph(this AnimationNode obj)
      => obj.ObserveSignal("removed_from_graph");
    public static IObservable<Unit> OnTreeChanged(this AnimationNode obj)
      => obj.ObserveSignal("tree_changed");
    public static IObservable<Unit> OnTrianglesUpdated(this AnimationNodeBlendSpace2D obj)
      => obj.ObserveSignal("triangles_updated");
    public static IObservable<Unit> OnAdvanceConditionChanged(this AnimationNodeStateMachineTransition obj)
      => obj.ObserveSignal("advance_condition_changed");
    public static IObservable<Godot.Mesh> OnMeshUpdated(this ARVRController obj)
      => obj.ObserveSignal<Godot.Mesh>("mesh_updated");
    public static IObservable<int> OnButtonRelease(this ARVRController obj)
      => obj.ObserveSignal<int>("button_release");
    public static IObservable<int> OnButtonPressed(this ARVRController obj)
      => obj.ObserveSignal<int>("button_pressed");
    public static IObservable<Godot.Mesh> OnMeshUpdated(this ARVRAnchor obj)
      => obj.ObserveSignal<Godot.Mesh>("mesh_updated");
    public static IObservable<Unit> OnFrameChanged(this Sprite3D obj)
      => obj.ObserveSignal("frame_changed");
    public static IObservable<Unit> OnFrameChanged(this AnimatedSprite3D obj)
      => obj.ObserveSignal("frame_changed");
    public static IObservable<Unit> OnRangeChanged(this Curve obj)
      => obj.ObserveSignal("range_changed");
    public static IObservable<Unit> OnMouseExited(this CollisionObject obj)
      => obj.ObserveSignal("mouse_exited");
    public static IObservable<Unit> OnMouseEntered(this CollisionObject obj)
      => obj.ObserveSignal("mouse_entered");
    public static IObservable<(Godot.Node Camera, Godot.InputEvent Event, Vector3 ClickPosition, Vector3 ClickNormal, int ShapeIdx)> OnInputEvent(this CollisionObject obj)
      => obj.ObserveSignal<Godot.Node, Godot.InputEvent, Vector3, Vector3, int>("input_event");
    public static IObservable<Godot.Node> OnBodyEntered(this RigidBody obj)
      => obj.ObserveSignal<Godot.Node>("body_entered");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int LocalShape)> OnBodyShapeEntered(this RigidBody obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_entered");
    public static IObservable<Unit> OnSleepingStateChanged(this RigidBody obj)
      => obj.ObserveSignal("sleeping_state_changed");
    public static IObservable<Godot.Node> OnBodyExited(this RigidBody obj)
      => obj.ObserveSignal<Godot.Node>("body_exited");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int LocalShape)> OnBodyShapeExited(this RigidBody obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_exited");
    public static IObservable<Godot.Area> OnAreaExited(this Area obj)
      => obj.ObserveSignal<Godot.Area>("area_exited");
    public static IObservable<(int AreaId, Godot.Area Area, int AreaShape, int SelfShape)> OnAreaShapeExited(this Area obj)
      => obj.ObserveSignal<int, Godot.Area, int, int>("area_shape_exited");
    public static IObservable<Godot.Node> OnBodyEntered(this Area obj)
      => obj.ObserveSignal<Godot.Node>("body_entered");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int AreaShape)> OnBodyShapeEntered(this Area obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_entered");
    public static IObservable<Godot.Area> OnAreaEntered(this Area obj)
      => obj.ObserveSignal<Godot.Area>("area_entered");
    public static IObservable<(int AreaId, Godot.Area Area, int AreaShape, int SelfShape)> OnAreaShapeEntered(this Area obj)
      => obj.ObserveSignal<int, Godot.Area, int, int>("area_shape_entered");
    public static IObservable<Godot.Node> OnBodyExited(this Area obj)
      => obj.ObserveSignal<Godot.Node>("body_exited");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int AreaShape)> OnBodyShapeExited(this Area obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_exited");
    public static IObservable<(string GroupName, Array Parameters)> OnBroadcast(this ProximityGroup obj)
      => obj.ObserveSignal<string, Array>("broadcast");
    public static IObservable<Godot.Camera> OnCameraExited(this VisibilityNotifier obj)
      => obj.ObserveSignal<Godot.Camera>("camera_exited");
    public static IObservable<Unit> OnScreenEntered(this VisibilityNotifier obj)
      => obj.ObserveSignal("screen_entered");
    public static IObservable<Godot.Camera> OnCameraEntered(this VisibilityNotifier obj)
      => obj.ObserveSignal<Godot.Camera>("camera_entered");
    public static IObservable<Unit> OnScreenExited(this VisibilityNotifier obj)
      => obj.ObserveSignal("screen_exited");
    public static IObservable<Unit> OnCurveChanged(this Path obj)
      => obj.ObserveSignal("curve_changed");
    public static IObservable<Unit> OnEditorRefreshRequest(this VisualShaderNode obj)
      => obj.ObserveSignal("editor_refresh_request");
    public static IObservable<Unit> OnInputTypeChanged(this VisualShaderNodeInput obj)
      => obj.ObserveSignal("input_type_changed");
    public static IObservable<Unit> OnFrameChanged(this Sprite obj)
      => obj.ObserveSignal("frame_changed");
    public static IObservable<Unit> OnTextureChanged(this Sprite obj)
      => obj.ObserveSignal("texture_changed");
    public static IObservable<Unit> OnFrameChanged(this AnimatedSprite obj)
      => obj.ObserveSignal("frame_changed");
    public static IObservable<Unit> OnAnimationFinished(this AnimatedSprite obj)
      => obj.ObserveSignal("animation_finished");
    public static IObservable<Unit> OnMouseExited(this CollisionObject2D obj)
      => obj.ObserveSignal("mouse_exited");
    public static IObservable<Unit> OnMouseEntered(this CollisionObject2D obj)
      => obj.ObserveSignal("mouse_entered");
    public static IObservable<(Godot.Node Viewport, Godot.InputEvent Event, int ShapeIdx)> OnInputEvent(this CollisionObject2D obj)
      => obj.ObserveSignal<Godot.Node, Godot.InputEvent, int>("input_event");
    public static IObservable<Unit> OnTextureChanged(this MeshInstance2D obj)
      => obj.ObserveSignal("texture_changed");
    public static IObservable<Godot.Node> OnBodyEntered(this RigidBody2D obj)
      => obj.ObserveSignal<Godot.Node>("body_entered");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int LocalShape)> OnBodyShapeEntered(this RigidBody2D obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_entered");
    public static IObservable<Unit> OnSleepingStateChanged(this RigidBody2D obj)
      => obj.ObserveSignal("sleeping_state_changed");
    public static IObservable<Godot.Node> OnBodyExited(this RigidBody2D obj)
      => obj.ObserveSignal<Godot.Node>("body_exited");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int LocalShape)> OnBodyShapeExited(this RigidBody2D obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_exited");
    public static IObservable<Unit> OnTextureChanged(this MultiMeshInstance2D obj)
      => obj.ObserveSignal("texture_changed");
    public static IObservable<Godot.Area2D> OnAreaExited(this Area2D obj)
      => obj.ObserveSignal<Godot.Area2D>("area_exited");
    public static IObservable<(int AreaId, Godot.Area2D Area, int AreaShape, int SelfShape)> OnAreaShapeExited(this Area2D obj)
      => obj.ObserveSignal<int, Godot.Area2D, int, int>("area_shape_exited");
    public static IObservable<Godot.Node> OnBodyEntered(this Area2D obj)
      => obj.ObserveSignal<Godot.Node>("body_entered");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int AreaShape)> OnBodyShapeEntered(this Area2D obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_entered");
    public static IObservable<Godot.Area2D> OnAreaEntered(this Area2D obj)
      => obj.ObserveSignal<Godot.Area2D>("area_entered");
    public static IObservable<(int AreaId, Godot.Area2D Area, int AreaShape, int SelfShape)> OnAreaShapeEntered(this Area2D obj)
      => obj.ObserveSignal<int, Godot.Area2D, int, int>("area_shape_entered");
    public static IObservable<Godot.Node> OnBodyExited(this Area2D obj)
      => obj.ObserveSignal<Godot.Node>("body_exited");
    public static IObservable<(int BodyId, Godot.Node Body, int BodyShape, int AreaShape)> OnBodyShapeExited(this Area2D obj)
      => obj.ObserveSignal<int, Godot.Node, int, int>("body_shape_exited");
    public static IObservable<Unit> OnScreenEntered(this VisibilityNotifier2D obj)
      => obj.ObserveSignal("screen_entered");
    public static IObservable<Godot.Viewport> OnViewportEntered(this VisibilityNotifier2D obj)
      => obj.ObserveSignal<Godot.Viewport>("viewport_entered");
    public static IObservable<Unit> OnScreenExited(this VisibilityNotifier2D obj)
      => obj.ObserveSignal("screen_exited");
    public static IObservable<Godot.Viewport> OnViewportExited(this VisibilityNotifier2D obj)
      => obj.ObserveSignal<Godot.Viewport>("viewport_exited");
    public static IObservable<Unit> OnBoneSetupChanged(this Skeleton2D obj)
      => obj.ObserveSignal("bone_setup_changed");
    public static IObservable<Unit> OnSettingsChanged(this TileMap obj)
      => obj.ObserveSignal("settings_changed");
    public static IObservable<Unit> OnReleased(this TouchScreenButton obj)
      => obj.ObserveSignal("released");
    public static IObservable<Unit> OnPressed(this TouchScreenButton obj)
      => obj.ObserveSignal("pressed");
    public static IObservable<Unit> OnTracksChanged(this Animation obj)
      => obj.ObserveSignal("tracks_changed");
    public static IObservable<Unit> OnTextureChanged(this StyleBoxTexture obj)
      => obj.ObserveSignal("texture_changed");
    public static IObservable<Unit> OnFinished(this AudioStreamPlayer obj)
      => obj.ObserveSignal("finished");
    public static IObservable<Unit> OnFinished(this AudioStreamPlayer3D obj)
      => obj.ObserveSignal("finished");
    public static IObservable<Unit> OnFinished(this AudioStreamPlayer2D obj)
      => obj.ObserveSignal("finished");
    public static IObservable<Unit> OnConnectedToServer(this SceneTree obj)
      => obj.ObserveSignal("connected_to_server");
    public static IObservable<Godot.Node> OnNodeConfigurationWarningChanged(this SceneTree obj)
      => obj.ObserveSignal<Godot.Node>("node_configuration_warning_changed");
    public static IObservable<Unit> OnConnectionFailed(this SceneTree obj)
      => obj.ObserveSignal("connection_failed");
    public static IObservable<Unit> OnPhysicsFrame(this SceneTree obj)
      => obj.ObserveSignal("physics_frame");
    public static IObservable<Unit> OnScreenResized(this SceneTree obj)
      => obj.ObserveSignal("screen_resized");
    public static IObservable<int> OnNetworkPeerDisconnected(this SceneTree obj)
      => obj.ObserveSignal<int>("network_peer_disconnected");
    public static IObservable<int> OnNetworkPeerConnected(this SceneTree obj)
      => obj.ObserveSignal<int>("network_peer_connected");
    public static IObservable<Godot.Node> OnNodeRemoved(this SceneTree obj)
      => obj.ObserveSignal<Godot.Node>("node_removed");
    public static IObservable<Godot.Node> OnNodeAdded(this SceneTree obj)
      => obj.ObserveSignal<Godot.Node>("node_added");
    public static IObservable<(string[] Files, int Screen)> OnFilesDropped(this SceneTree obj)
      => obj.ObserveSignal<string[], int>("files_dropped");
    public static IObservable<Unit> OnIdleFrame(this SceneTree obj)
      => obj.ObserveSignal("idle_frame");
    public static IObservable<Unit> OnServerDisconnected(this SceneTree obj)
      => obj.ObserveSignal("server_disconnected");
    public static IObservable<Godot.Node> OnNodeRenamed(this SceneTree obj)
      => obj.ObserveSignal<Godot.Node>("node_renamed");
    public static IObservable<Unit> OnTreeChanged(this SceneTree obj)
      => obj.ObserveSignal("tree_changed");
    public static IObservable<(object Id, object Meta)> OnGlobalMenuAction(this SceneTree obj)
      => obj.ObserveSignal<object, object>("global_menu_action");
    public static IObservable<Unit> OnTimeout(this SceneTreeTimer obj)
      => obj.ObserveSignal("timeout");
    public static IObservable<string> OnMainScreenChanged(this EditorPlugin obj)
      => obj.ObserveSignal<string>("main_screen_changed");
    public static IObservable<string> OnSceneClosed(this EditorPlugin obj)
      => obj.ObserveSignal<string>("scene_closed");
    public static IObservable<Godot.Node> OnSceneChanged(this EditorPlugin obj)
      => obj.ObserveSignal<Godot.Node>("scene_changed");
    public static IObservable<Godot.Resource> OnResourceSaved(this EditorPlugin obj)
      => obj.ObserveSignal<Godot.Resource>("resource_saved");
    public static IObservable<Unit> OnSelectionChanged(this EditorSelection obj)
      => obj.ObserveSignal("selection_changed");
    public static IObservable<string[]> OnFilesSelected(this EditorFileDialog obj)
      => obj.ObserveSignal<string[]>("files_selected");
    public static IObservable<string> OnDirSelected(this EditorFileDialog obj)
      => obj.ObserveSignal<string>("dir_selected");
    public static IObservable<string> OnFileSelected(this EditorFileDialog obj)
      => obj.ObserveSignal<string>("file_selected");
    public static IObservable<string> OnPreviewInvalidated(this EditorResourcePreview obj)
      => obj.ObserveSignal<string>("preview_invalidated");
    public static IObservable<Unit> OnSettingsChanged(this EditorSettings obj)
      => obj.ObserveSignal("settings_changed");
    public static IObservable<string[]> OnResourcesReimported(this EditorFileSystem obj)
      => obj.ObserveSignal<string[]>("resources_reimported");
    public static IObservable<bool> OnSourcesChanged(this EditorFileSystem obj)
      => obj.ObserveSignal<bool>("sources_changed");
    public static IObservable<Unit> OnFilesystemChanged(this EditorFileSystem obj)
      => obj.ObserveSignal("filesystem_changed");
    public static IObservable<string[]> OnResourcesReload(this EditorFileSystem obj)
      => obj.ObserveSignal<string[]>("resources_reload");
    public static IObservable<Godot.Script> OnEditorScriptChanged(this ScriptEditor obj)
      => obj.ObserveSignal<Godot.Script>("editor_script_changed");
    public static IObservable<Godot.Script> OnScriptClose(this ScriptEditor obj)
      => obj.ObserveSignal<Godot.Script>("script_close");
    public static IObservable<string> OnPropertyEdited(this EditorInspector obj)
      => obj.ObserveSignal<string>("property_edited");
    public static IObservable<int> OnObjectIdSelected(this EditorInspector obj)
      => obj.ObserveSignal<int>("object_id_selected");
    public static IObservable<string> OnPropertySelected(this EditorInspector obj)
      => obj.ObserveSignal<string>("property_selected");
    public static IObservable<Unit> OnRestartRequested(this EditorInspector obj)
      => obj.ObserveSignal("restart_requested");
    public static IObservable<string> OnPropertyKeyed(this EditorInspector obj)
      => obj.ObserveSignal<string>("property_keyed");
    public static IObservable<(Object Res, string Prop)> OnResourceSelected(this EditorInspector obj)
      => obj.ObserveSignal<Object, string>("resource_selected");
    public static IObservable<(string Property, bool Checked)> OnPropertyToggled(this EditorInspector obj)
      => obj.ObserveSignal<string, bool>("property_toggled");
    public static IObservable<Godot.Script> OnScriptCreated(this ScriptCreateDialog obj)
      => obj.ObserveSignal<Godot.Script>("script_created");
    public static IObservable<(string Property, int Id)> OnObjectIdSelected(this EditorProperty obj)
      => obj.ObserveSignal<string, int>("object_id_selected");
    public static IObservable<(string Property, string Bool)> OnPropertyChecked(this EditorProperty obj)
      => obj.ObserveSignal<string, string>("property_checked");
    public static IObservable<(string Property, object Value)> OnPropertyKeyedWithValue(this EditorProperty obj)
      => obj.ObserveSignal<string, object>("property_keyed_with_value");
    public static IObservable<string> OnPropertyKeyed(this EditorProperty obj)
      => obj.ObserveSignal<string>("property_keyed");
    public static IObservable<(string Property, object Value)> OnPropertyChanged(this EditorProperty obj)
      => obj.ObserveSignal<string, object>("property_changed");
    public static IObservable<(string Path, Godot.Resource Resource)> OnResourceSelected(this EditorProperty obj)
      => obj.ObserveSignal<string, Godot.Resource>("resource_selected");
    public static IObservable<(string Path, int FocusableIdx)> OnSelected(this EditorProperty obj)
      => obj.ObserveSignal<string, int>("selected");
    public static IObservable<(string[] Properties, Array Value)> OnMultiplePropertiesChanged(this EditorProperty obj)
      => obj.ObserveSignal<string[], Array>("multiple_properties_changed");
    public static IObservable<Unit> OnDisplayModeChanged(this FileSystemDock obj)
      => obj.ObserveSignal("display_mode_changed");
    public static IObservable<string> OnInherit(this FileSystemDock obj)
      => obj.ObserveSignal<string>("inherit");
    public static IObservable<(string OldFile, string NewFile)> OnFilesMoved(this FileSystemDock obj)
      => obj.ObserveSignal<string, string>("files_moved");
    public static IObservable<string> OnFolderRemoved(this FileSystemDock obj)
      => obj.ObserveSignal<string>("folder_removed");
    public static IObservable<string[]> OnInstance(this FileSystemDock obj)
      => obj.ObserveSignal<string[]>("instance");
    public static IObservable<(string OldFolder, string NewFile)> OnFolderMoved(this FileSystemDock obj)
      => obj.ObserveSignal<string, string>("folder_moved");
    public static IObservable<string> OnFileRemoved(this FileSystemDock obj)
      => obj.ObserveSignal<string>("file_removed");
    public static IObservable<object> OnCompleted(this GDScriptFunctionState obj)
      => obj.ObserveSignal<object>("completed");
    public static IObservable<Vector3> OnCellSizeChanged(this GridMap obj)
      => obj.ObserveSignal<Vector3>("cell_size_changed");
    public static IObservable<Unit> OnPortsChanged(this VisualScriptNode obj)
      => obj.ObserveSignal("ports_changed");
    public static IObservable<(string Function, int Id)> OnNodePortsChanged(this VisualScript obj)
      => obj.ObserveSignal<string, int>("node_ports_changed");
    public static IObservable<(string Media, int Index, string Name)> OnIceCandidateCreated(this WebRTCPeerConnection obj)
      => obj.ObserveSignal<string, int, string>("ice_candidate_created");
    public static IObservable<(string Type, string Sdp)> OnSessionDescriptionCreated(this WebRTCPeerConnection obj)
      => obj.ObserveSignal<string, string>("session_description_created");
    public static IObservable<Object> OnDataChannelReceived(this WebRTCPeerConnection obj)
      => obj.ObserveSignal<Object>("data_channel_received");
    public static IObservable<(int Code, string Reason)> OnServerCloseRequest(this WebSocketClient obj)
      => obj.ObserveSignal<int, string>("server_close_request");
    public static IObservable<string> OnConnectionEstablished(this WebSocketClient obj)
      => obj.ObserveSignal<string>("connection_established");
    public static IObservable<Unit> OnDataReceived(this WebSocketClient obj)
      => obj.ObserveSignal("data_received");
    public static IObservable<Unit> OnConnectionError(this WebSocketClient obj)
      => obj.ObserveSignal("connection_error");
    public static IObservable<bool> OnConnectionClosed(this WebSocketClient obj)
      => obj.ObserveSignal<bool>("connection_closed");
    public static IObservable<(int Id, int Code, string Reason)> OnClientCloseRequest(this WebSocketServer obj)
      => obj.ObserveSignal<int, int, string>("client_close_request");
    public static IObservable<int> OnDataReceived(this WebSocketServer obj)
      => obj.ObserveSignal<int>("data_received");
    public static IObservable<(int Id, string Protocol)> OnClientConnected(this WebSocketServer obj)
      => obj.ObserveSignal<int, string>("client_connected");
    public static IObservable<(int Id, bool WasCleanClose)> OnClientDisconnected(this WebSocketServer obj)
      => obj.ObserveSignal<int, bool>("client_disconnected");
    public static IObservable<int> OnPeerPacket(this WebSocketMultiplayerPeer obj)
      => obj.ObserveSignal<int>("peer_packet");
  }
}