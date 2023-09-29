# SQL Testing

## Usage:
### Transactions:
- startSave() : void
- commitSave() : void
- rollbackSave() : void

*A transaction MUST be started in order to use any other methods. A single transaction can only read or write not both. Data is only saved after a commit.*

### Saves:
- addSave(string savename) : bool
- selectSave(string savename) : bool
- exitSave() : void

### Tile Item:
- addTileItem(string itemName) : bool
- getTileItem(string itemName) : int

### Tile Date:
- addTile(int xPos, int yPos, int type) : int
- getTile(int xPos, int yPos) : int

### Resource:
- addResource(string resourcesName) : bool
- getResource(string resourcesName) : int

### Units:
- getUnits() : List<Unit>
- addUnit(float xPos, float yPos, float xTarget, float yTarget, float health) : int
- setUnit(int id, float xPos, float yPos, float xTarget, float yTarget, float health) : void

### Placeables:
- getPlaceables() : List<Placeable>
- getNaturalPlaceables() : List<Placeable>
- getUnnaturalPlaceables() : List<Placeable>
- addPlaceable(int tileItemID, float xPos, float yPos, float xTarget, float yTarget, float health, int natural) : int
- setPlaceable(int id, float xPos, float yPos, float health, float heading) : void

### Inventory
- addItemToInventory(int id, int value) : void
- updateInventory(int id, int value) : void
- amountInInventory(int id) : int

### Tables:
- AddTableIfMissing(string name, string columns) : void