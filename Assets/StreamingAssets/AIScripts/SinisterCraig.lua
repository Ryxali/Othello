-- TODO do queue -> search


function searchForBest()
	local queue = {
		
	}
end

function getPossibilities(board)
	-- return table containing possible moves for board
	local options = {}
	local nOptions = 1
	for x = 0, 7 do
		for y = 0, 7 do
			if ai.currentBoard.isValidPlacementLocation(ai.owner, x, y) then
				local tmp = ai.currentBoard.copy()
				tmp.placePawnOnTile(ai.owner, x, y)
				options[nOptions] = tmp
				nOptions = nOptions + 1
			end
		end
	end
	options.size = nOptions
	return options
end

function onCalculateBestMove()
	local solution = {}
	local possibilities = getPossibilities(ai.currentBoard)
	for i = 1, possibilities.size do
		local possibility = getPossibilities(possibility[i])
		for j = 1, possibility.size do
			
		end
	end
	return solution
end