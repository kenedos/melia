Melia.Override("GET_REPAIR_PRICE", function(original, item, fillValue, taxRate)
    -- Base price
    local basePrice = 100

    -- Level multiplier calculation (smoothed exponential-to-linear curve)
    local level = TryGetProp(item, "UseLv") or 0
    local baseMultiplier = 1
    local exponentialFactor = 0.04
    local linearFactor = 25
    local transitionLevel = 100
    local transitionSmoothness = 50

    local exponentialPart = math.pow(1 + exponentialFactor, level)
    local linearPart = 1 + (level / linearFactor)
    local transitionFactor = 1 / (1 + math.exp(-(level - transitionLevel) / transitionSmoothness))
    local levelMultiplier = baseMultiplier * (exponentialPart * (1 - transitionFactor) + linearPart * transitionFactor)

    -- Grade multiplier (20% per grade linear increase)
    local itemGrade = TryGetProp(item, "ItemGrade") or 1
    local gradeMultiplier = 1 + (itemGrade - 1) * 0.2

    -- Type multiplier
    local typeMultiplier = 1.0
    local equipType = TryGetProp(item, "GroupName")
    if equipType == "Weapon" then
        typeMultiplier = 1.5
    elseif equipType == "SubWeapon" then
        typeMultiplier = 1.4
    elseif equipType == "Armor" then
        typeMultiplier = 1.2
    end

    -- Calculate final repair cost
    local repairCost = basePrice * levelMultiplier * gradeMultiplier * typeMultiplier

    -- Round down to nearest integer
    repairCost = math.floor(repairCost)

    -- Apply tax rate if provided
    if taxRate ~= nil then
        repairCost = tonumber(CALC_PRICE_WITH_TAX_RATE(repairCost, taxRate))
    end

    return repairCost
end)