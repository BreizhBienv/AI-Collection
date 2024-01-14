# AI-Planning

## Descrition
An exercise done in my 3rd year at Isart Digital in January 2024. The purpose of the exercise was to discover and implement the AI planning algorithm. The project was made alone and using Unity.

## How the AI works
The AI is a Miner, he must fetch a pickaxe et mine an ore chunk with it, or with it hands, bring the ore the the furnace, process it, and once an ingot is ready, fetch it and bring it to the chest.

Different world state and action are used to produce a plan.

### World State
We have two kind of world state, one managed by the world, and one perceived each AI individually.
In our case, we have:

World State  | Perceived World State
------------- | -------------
 Available_OreChunk</br>Availabe_Furnace</br>Available_Ingot</br>Available_Pickaxe | Near_Pickaxe</br>Near_OreChunk</br>Near_Furnace</br>Near_Chest</br>Has_PickaxeHas_Ore</br>Has_Ingot</br>Processed_Ore</br>Stored_Ingot

 Finally, a world state can also be the goal of the plan of the AI, here, it would be `Processed_Ore` and `Stored_Ingot`.

 ### Actions
 To make a plan, we need actions, an action possess a cost, and conditions needed to execute, those conditions are world state.</br>
 By having different cost, we allow the AI to take choices more efficient, as an exemple, if mining with hands costs 5 and mining with a pickaxe cost 1, the plan chosed by the AI will incorporate fetching the pickaxe rather than just mining with his hands.

 Here are our actions:

 Process_Ore  | Store_Ingot
------------- | -------------
MoveTo_Pickaxe</br>MoveTo_OreChunk</br>MoveTo_Furnace</br>Equip_Pickaxe</br>MineOre_WithHands</br>MineOre_WithPickaxe</br>Process_Ore | MoveTo_Furnace</br>MoveTo_Chest</br>Retrieve_Ingot</br>Store_Ingot
