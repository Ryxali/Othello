

function onCalculateBestMove()
	local possiblilities = {}
	local nPossibilities = 1
	for x = 0, 7 do
		for y = 0, 7 do
			if ai.currentBoard.isValidPlacementLocation(ai.owner, x, y) then
				possiblilities[nPossibilities] = {x, y}
				nPossibilities = nPossibilities + 1
			end
		end
	end
	math.randomseed(unity.time())
	local rand = math.random(nPossibilities-1)
	return possiblilities[rand]
end