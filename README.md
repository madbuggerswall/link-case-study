### ReadMe

#### Puzzle Level Initialization

The `PuzzleLevelInitializer` is responsible for setting up the puzzle level at runtime using data from a `PuzzleLevelDefinition` asset.

To edit or create new levels, you can find `PuzzleLevelDefinition` assets at _Assets/Data/Definitions/PuzzleLevels/_

To try a different level, simply assign a new `PuzzleLevelDefinition` to the `PuzzleLevelInitializer` component in the Inspector.

#### UI Layout Note

This case study was developed for **portrait orientation** (810x1755), so UI elements may **overlap with the level** when run in the default **16:9 landscape view** . For best results, test and play in a **portrait aspect ratio**.

#### Known Issues

Shuffling the board **after a turn** results in a `InvalidOperationException: Collection was modified` from `TweenManager`,  even though the same shuffling logic works correctly during level start.
