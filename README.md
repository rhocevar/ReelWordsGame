# Reel Words

A console-based word game inspired by slot machines. Players form words from letters displayed on rotating reels, earning points based on letter values.

## Gameplay

The game presents 6 reels, each containing a sequence of letters. The currently visible letters (one per reel) form the player's **rack** -- the available tiles for that turn.

- Use the rack letters in **any order** to form a valid English word.
- Each tile can only be used **once** per word (duplicate letters on the rack allow that letter to appear multiple times).
- When a word is played, each used reel **rotates**: the used letter cycles to the back of its reel and the next letter in the sequence becomes visible.
- Unused reels stay in place.
- Points are awarded based on the sum of each played letter's score value (similar to Scrabble scoring).

The reels start at random positions each game, simulating a slot machine.

## Project Structure

```
ReelWordsGame/
├── Program/
│   ├── ReelWords/                        # Main application
│   │   ├── Program.cs                    # Entry point
│   │   ├── Config/                       # Language configuration
│   │   ├── Data/
│   │   │   ├── Trie.cs                   # Trie data structure for word lookup
│   │   │   ├── ReelWordsData.cs          # Game data container
│   │   │   └── Loaders/                  # Data loading (dictionary, reels, scores)
│   │   ├── Game/
│   │   │   ├── GameManager.cs            # Core game loop and orchestration
│   │   │   ├── Rack.cs                   # Reel management and word play logic
│   │   │   └── Tile.cs                   # Letter tile with score
│   │   ├── Validation/                   # Word validation rules
│   │   ├── View/                         # Console UI abstraction
│   │   └── Utilities/                    # Helper methods
│   ├── ReelWordsTests/                   # Unit tests (xUnit)
│   └── ReelWords.sln
└── Resources/
    ├── american-english-large.txt        # English dictionary (~164k words)
    ├── reels.txt                         # Reel letter sequences (6 rows x 7 columns)
    └── scores.txt                        # Letter-to-score mapping (a-z)
```

## Key Components

### Trie

A prefix-tree data structure used for fast dictionary lookups. Supports **Insert**, **Search**, and **Delete** operations with O(m) time complexity, where m is the word length. The entire English dictionary is loaded into the Trie at startup.

### Rack & Reels

Each reel is implemented as a queue of tiles. The rack displays the front tile of each reel. When a letter is played, its tile is dequeued and enqueued back to the end of its reel, advancing that reel to the next letter.

### Word Validation

Words are validated in three stages:
1. **Format validation** -- must be 2-7 lowercase letters (a-z only).
2. **Dictionary validation** -- must exist in the Trie (loaded from the dictionary file).
3. **Rack validation** -- all letters must be available on the current rack.

### Data Loading

Game data is loaded asynchronously from three resource files in parallel:
- **Dictionary** -- filtered to words between 2 and 7 characters (matching rack capacity).
- **Reels** -- parsed into tile queues and shuffled for random starting positions.
- **Scores** -- mapped to each tile after loading.

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later

## Build & Run

```bash
# Build the solution
dotnet build Program/ReelWords.sln

# Run the game
dotnet run --project Program/ReelWords/ReelWords.csproj
```

## Running Tests

```bash
dotnet test Program/ReelWords.sln
```

The test suite covers:
- Trie operations (insert, search, delete, duplicate handling)
- Data loading from resource files
- Rack mechanics (successful plays, failed plays, reel rotation)
- Word validation (valid/invalid inputs)

## How to Play

1. Run the game. The dictionary loads and your initial rack is displayed.
2. Type a word using the available letters and press Enter.
3. If the word is valid, you earn points and the used reels rotate to new letters.
4. If the word is invalid or uses unavailable letters, you are informed and can try again.
5. Type `0` to quit.
