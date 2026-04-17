-- Laima custom propertyshop injection.
-- Lets the server define a propertyshop's item list at runtime without
-- requiring a <Shop> entry in propertyshop.xml.

M_CUSTOM_PROPERTY_SHOPS = M_CUSTOM_PROPERTY_SHOPS or {}

local function build_shopinfo(items, pointScript)
	local shop = {
		_items = items or {},
		_pointScript = pointScript or "GET_PVP_POINT",
	}

	function shop:GetItemCount()
		return #self._items
	end

	function shop:GetPointScript()
		return self._pointScript
	end

	function shop:GetItemByIndex(i)
		local d = self._items[i + 1]
		if d == nil then
			return nil
		end

		local item = {
			count = d.count or 1,
			price = d.price or 0,
			dailyBuyLimit = d.dailyBuyLimit or 0,
			_name = d.name,
			_accountNeedProperty = d.accountNeedProperty or "",
			_scriptName = d.scriptName or "None",
		}

		function item:GetScriptName() return self._scriptName end
		function item:GetItemName() return self._name end
		function item:GetAccountNeedProperty() return self._accountNeedProperty end

		return item
	end

	return shop
end

-- Function executed from the server to install a custom shop.
-- items format: { { name = "Artefact_XXX", count = 1, price = 30 }, ... }
function M_SET_CUSTOM_PROPERTY_SHOP(items, shopName, pointScript)
	M_CUSTOM_PROPERTY_SHOPS[shopName] = build_shopinfo(items, pointScript)
end

-- Chunked receive bridge used by Melia.Comm.ExecData.
function M_RECV_CUSTOM_PROPERTY_SHOP(data)
	local shopName = data.shopName
	local pointScript = data.pointScript
	local items = data.items or {}
	M_SET_CUSTOM_PROPERTY_SHOP(items, shopName, pointScript)
end

-- Intercept gePropertyShop.Get to serve registered shops.
Melia.OverrideIn(gePropertyShop, "Get", function(original, shopName)
	local custom = M_CUSTOM_PROPERTY_SHOPS[shopName]
	if custom ~= nil then
		return custom
	end
	return original(shopName)
end)
