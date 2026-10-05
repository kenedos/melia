function MELIA_COLLECTION_UPDATE_CONTROL(control)
    if control == nil then
        return
    end

    local ok, collectionId = pcall(function()
        return control:GetUserIValue("COLLECTION_TYPE")
    end)

    if ok and collectionId ~= nil and collectionId > 0 then
        local magicList = GET_CHILD_RECURSIVELY(control, "magicList")

        if magicList ~= nil then
            magicList = tolua.cast(magicList, "ui::CRichText")
            magicList:SetText(MELIA_COLLECTION_EFFECT_DESC(collectionId))
        end
    end

    local childOk, childCount = pcall(function()
        return control:GetChildCount()
    end)

    if childOk then
        for i = 0, childCount - 1 do
            MELIA_COLLECTION_UPDATE_CONTROL(control:GetChildByIndex(i))
        end
    end
end

function MELIA_COLLECTION_REFRESH()
    local frame = ui.GetFrame("collection")

    if frame == nil or frame:IsVisible() == 0 then
        return
    end

    local collectionControl = GET_CHILD(frame, "col")

    if collectionControl ~= nil then
        MELIA_COLLECTION_UPDATE_CONTROL(collectionControl)
    end
end

function MELIA_COLLECTION_INSTALL()
    if MELIA_COLLECTION_OVERRIDES_INSTALLED == true then
        return
    end

    if type(GET_COLLECTION_EFFECT_DESC) ~= "function" or
        type(UPDATE_COLLECTION_LIST) ~= "function" then
        ReserveScript("MELIA_COLLECTION_INSTALL()", 0.5)
        return
    end

    MELIA_COLLECTION_OVERRIDES_INSTALLED = true

    local originalEffectDesc = GET_COLLECTION_EFFECT_DESC
    GET_COLLECTION_EFFECT_DESC = function(collectionId)
        return MELIA_COLLECTION_EFFECT_DESC(collectionId)
    end

    local originalUpdateList = UPDATE_COLLECTION_LIST
    UPDATE_COLLECTION_LIST = function(...)
        local result = originalUpdateList(...)
        ReserveScript("MELIA_COLLECTION_REFRESH()", 0.1)
        return result
    end

    ui.SysMsg("[Collection] Updated values interface installed.")

    local frame = ui.GetFrame("collection")

    if frame ~= nil and frame:IsVisible() == 1 then
        UPDATE_COLLECTION_LIST(frame)
        ReserveScript("MELIA_COLLECTION_REFRESH()", 0.2)
    end
end

ReserveScript("MELIA_COLLECTION_INSTALL()", 0.5)
