-- Streaming install: lets the server push a large item list across
-- multiple ZC_EXEC_CLIENT_SCP packets (each kept under the packet limit).
-- Sequence: M_CPS_BEGIN -> N x M_CPS_ADD -> M_CPS_END.

M_CPS_PENDING = M_CPS_PENDING or {}

function M_CPS_BEGIN(shopName)
	M_CPS_PENDING[shopName] = {}
end

function M_CPS_ADD(shopName, items)
	local t = M_CPS_PENDING[shopName]
	if t == nil then
		t = {}
		M_CPS_PENDING[shopName] = t
	end
	for i = 1, #items do
		t[#t + 1] = items[i]
	end
end

function M_CPS_END(shopName, pointScript)
	local items = M_CPS_PENDING[shopName] or {}
	M_CUSTOM_PROPERTY_SHOPS[shopName] = M_BUILD_PROPERTY_SHOPINFO(items, pointScript)
	M_CPS_PENDING[shopName] = nil
end

-- Aliases one registered shop under another name, so the UI frame opens
-- against a known client-side shop entry while serving a different item list.
function M_CPS_ALIAS(aliasName, sourceShopName)
	M_CUSTOM_PROPERTY_SHOPS[aliasName] = M_CUSTOM_PROPERTY_SHOPS[sourceShopName]
end
