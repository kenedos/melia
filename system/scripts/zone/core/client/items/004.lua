Melia.Override("GET_APPRAISAL_PRICE", function(original, item, SellPrice, taxRate)
    SellPrice = TryGetProp(item,"SellPrice");
    local lv = TryGetProp(item,"UseLv");
    local grade = TryGetProp(item,"ItemGrade")
    local priceRatio = 10;
    if lv == nil then
        return 0;
    end

    if SellPrice == nil then
        if grade <= 2 then
            SellPrice = lv * priceRatio
        elseif grade > 2 then 
            SellPrice = math.floor((lv * priceRatio) * 1.5)
        else
            return;
        end
    end

    if taxRate ~= nil then
        SellPrice = tonumber(CALC_PRICE_WITH_TAX_RATE(SellPrice, taxRate))
    end
    return SellPrice;
end)