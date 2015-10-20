-- TODO do queue -> search


function searchForBest()
	local queue = {
		
	}
end

function getPossibilities(board)
	-- return table containing possible moves for board
	local options = {option = {}, nOptions = 0}
	for x = 0, 7 do
		for y = 0, 7 do
			if ai.currentBoard.isValidPlacementLocation(ai.owner, x, y) then
				options.nOptions = options.nOptions + 1
				local tmp = board.copy()
				tmp.placePawnOnTile(ai.owner, x, y)
				options.option[options.nOptions] = {}
				options.option[options.nOptions].board = tmp
				
				options.option[options.nOptions].move = {}
				options.option[options.nOptions].move.x = x
				options.option[options.nOptions].move.y = y
				
			end
		end
	end
	--options.size = nOptions
	return options
end

function onCalculateBestMove()
	local solution = {}
	local possibilities = getPossibilities(ai.currentBoard)
	local best = {move = {x = 0, y = 0}, value = 1000000}
	for i = 1, possibilities.nOptions do
		local possibility = getPossibilities(possibilities.option[i].board)
		local totalScore = 0
		for j = 1, possibility.nOptions do
			local score = possibility.option[j].board.getScore()
			totalScore = totalScore + score[ai.opponent]
		end
		local curScore = possibilities.option[i].board.getScore()
		totalScore = (totalScore - curScore[ai.opponent]) / possibility.nOptions
		unity.print(totalScore)
		if totalScore < best.value then
			best.move = possibility.option[i].move
			best.value = totalScore
		end
	end
	unity.print("Best " .. best.value)
	unity.print("----------------")
	solution[1] = 0
	solution[2] = 1
	return best.move
end