﻿namespace RpgGame.Models
{
    using RLNET;
    using RogueSharp;
    using RpgGame.Interfaces;
    using RpgGame.Utilities;

    public abstract class Character : ICharacter, IDrawable, IScheduleable
    {
        protected Character(string name, int awareness,
            int attack, int attackChance,
            int defence, int defenceChance,
            int gold,
            int health, int maxHealth, int speed,
            RLColor color, char symbol, int x, int y)
        {
            this.Name = name;
            this.Awareness = awareness;

            this.Attack = attack;
            this.AttackChance = attackChance;
            this.Defense = defence;
            this.DefenseChance = defenceChance;

            this.Gold = gold;
            this.Health = health;
            this.MaxHealth = maxHealth;
            this.Speed = speed;

            this.Color = color;
            this.Symbol = symbol;
            this.X = x;
            this.Y = y;

            this.Steps = 0;
        }

        public string Name { get; set; }
        public int Awareness { get; set; }
        public int Steps { get; set; }
        public int Attack { get; set; }
        public int AttackChance { get; set; }
        public int Defense { get; set; }
        public int DefenseChance { get; set; }
        public int Gold { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Speed { get; set; }
        public int Time => this.Speed;

        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole console, IMap map)
        {
            if (!map.GetCell(this.X, this.Y).IsExplored)
            {
                return;
            }

            if (map.IsInFov(this.X, this.Y))
            {
                console.Set(this.X, this.Y, this.Color, Colors.FloorBackgroundFov, this.Symbol);
            }
            else
            {
                console.Set(this.X, this.Y, Colors.Floor, Colors.FloorBackground, '.');
            }
        }
    }
}