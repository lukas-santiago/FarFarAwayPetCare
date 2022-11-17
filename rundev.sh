cd API/Application && dotnet run &
P1=$!
# cd Web/desafio-distribuicao-dos-lucros-react && npm run dev &
# P2=$!
wait $P1 #$P2