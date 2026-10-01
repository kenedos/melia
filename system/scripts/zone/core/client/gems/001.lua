-- Override the socket pricing function, in case we want different prices.
Melia.Override("GET_MAKE_SOCKET_PRICE", function(original, itemlv, grade, curcnt, taxRate)

	local clslist, cnt = GetClassList("socketprice");
    local gradRatio = { 1.2, 1, 0.5, 0.4, 0.3, 0.1 }
    local itemGradeRatio = 1;
    local priceMultiplier = 2;
    local basePrice = 10000;
    if curcnt >= 1 then
        itemGradeRatio = gradRatio[grade]
    end
    for i = 0, cnt - 1 do

        local cls = GetClassByIndexFromList(clslist, i);

        if cls.Lv == itemlv then
            local priceRatio =(curcnt + 1);
            local ret = SyncFloor(basePrice + cls.NewSocketPrice * priceMultiplier * (priceRatio ^ 2 / itemGradeRatio));
            if taxRate ~= nil then
                ret = tonumber(CALC_PRICE_WITH_TAX_RATE(ret, taxRate))
            end
            return ret
        end
    end

    return 0;
	
end)
