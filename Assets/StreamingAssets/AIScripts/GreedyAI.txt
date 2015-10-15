-- TODO do queue -> search


function searchForBest()
	local queue = {
		
	}
end



function onCalculateBestMove()
	local solution = {}
	local bestScore = ai.currentBoard.getScore()
	for x = 0, 7 do
		for y = 0, 7 do
			if ai.currentBoard.isValidPlacementLocation(ai.owner, x, y) then
				local nGen = ai.currentBoard.copy()
				nGen.placePawnOnTile(ai.owner, x, y)
				local score = nGen.getScore()
				
				if score[ai.owner] > bestScore[ai.owner] then
					bestScore = score
					solution[1] = x
					solution[2] = y
				end
				
			end
		end
	end
	return solution
end