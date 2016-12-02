﻿namespace RpgGame.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using RLNET;
    using RogueSharp;

    public class DungeonMap : Map
    {
        private readonly List<Monster> monsters;
        private List<Room> rooms;

        public DungeonMap()
        {
            this.monsters = new List<Monster>();
            this.rooms = new List<Room>();
        }

        public List<Room> Rooms
        {
            get { return this.rooms; }
        }

        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            foreach (Cell cell in this.GetAllCells())
            {
                this.SetConsoleSymbolForCell(mapConsole, cell);
            }

            int i = 0;

            foreach (Monster monster in this.monsters)
            {
                monster.Draw(mapConsole, this);

                if (this.IsInFov(monster.X, monster.Y))
                {
                    monster.DrawStats(statConsole, i);
                    i++;
                }
            }
        }

        public void UpdatePlayerFieldOfView()
        {
            Player player = Engine.Player;

            this.ComputeFov(player.X, player.Y, player.Awareness, true);

            foreach (Cell cell in this.GetAllCells())
            {
                if (this.IsInFov(cell.X, cell.Y))
                {
                    this.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        public bool SetActorPosition(Character character, int x, int y)
        {
            if (GetCell(x, y).IsWalkable)
            {
                this.SetIsWalkable(character.X, character.Y, true);

                character.X = x;
                character.Y = y;

                this.SetIsWalkable(character.X, character.Y, false);

                if (character is Player)
                {
                    this.UpdatePlayerFieldOfView();
                }

                return true;
            }

            return false;
        }

        public void AddPlayer(Player player)
        {
            Engine.Player = player;
            this.SetIsWalkable(player.X, player.Y, false);
            this.UpdatePlayerFieldOfView();
        }

        public void AddMonster(Monster monster)
        {
            this.monsters.Add(monster);
            this.SetIsWalkable(monster.X, monster.Y, false);
            Engine.SchedulingSystem.Add(monster);
        }

        public void RemoveMonster(Monster monster)
        {
            this.monsters.Remove(monster);
            this.SetIsWalkable(monster.X, monster.Y, true);
            Engine.SchedulingSystem.Remove(monster);
        }

        public Monster GetMonsterAt(int x, int y)
        {
            return this.monsters.FirstOrDefault(m => m.X == x && m.Y == y);
        }

        public Point GetRandomWalkableLocationInRoom(Room room)
        {
            if (this.DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Engine.Random.Next(1, room.DungeonRoom.Width - 2) + room.DungeonRoom.X;
                    int y = Engine.Random.Next(1, room.DungeonRoom.Height - 2) + room.DungeonRoom.Y;
                    if (this.IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return null;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = GetCell(x, y);
            this.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        private bool DoesRoomHaveWalkableSpace(Room room)
        {
            for (int x = 1; x <= room.DungeonRoom.Width - 2; x++)
            {
                for (int y = 1; y <= room.DungeonRoom.Height - 2; y++)
                {
                    if (this.IsWalkable(x + room.DungeonRoom.X, y + room.DungeonRoom.Y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            if (!cell.IsExplored)
            {
                return;
            }

            if (this.IsInFov(cell.X, cell.Y))
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }
    }
}