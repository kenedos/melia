Melia.Hook("SET_MONGEN_NPC_VISIBLE", function(original, result, picture, mapprop, MonProp)
    picture:ShowWindow(1)
    SET_MONGEN_NPC_VISIBLE_BY_MGAME(picture);
end)