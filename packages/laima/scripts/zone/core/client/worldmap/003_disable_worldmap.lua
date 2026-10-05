Melia.Override("UI_TOGGLE_WORLDMAP", function(original)
    if app.IsBarrackMode() == true then
        return
    end
    ui.ToggleFrame('simplemap')
end)