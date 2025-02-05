using System;
using System.Collections;
using System.Collections.Generic;

public class Program
{
    public bool winput = false; // 잘못된 입력
    public string name = "Chad";
    public Player player;
    public Inventory inventory;
    public Status status;
    public Shop shop;

    public Program()
    {
        inventory = new Inventory(this);
        status = new Status(this);
        shop = new Shop(this);
    }

    static void Main()
    {
        Console.Write("\x1b[48;2;30;30;70m");
        Console.Write("\x1b[38;2;255;255;255m");
        Console.Clear();

        Program program = new Program();
        program.createPlayer();
        program.home();

        Console.Write("\x1b[0m");
        Console.ReadLine();
    }

    public void createPlayer()
    {
        Console.WriteLine("당신의 이름은 무엇입니까?");
        name = Console.ReadLine();

        
        player = new Player
        {
            Level = 1,
            Name = name,
            Job = "전사",
            Attack = 10,
            Defense = 5,
            Health = 100,
            Gold = 1500000000
        };
    }

    public void home()
    {
        if (winput == false)
        {
            Console.Clear();
            Console.WriteLine($"스파르타 마을에 오신 {player.Name}님 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
        }

        Console.WriteLine("");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("");
        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.Write(">>");
        winput = false;
        string inPut = Console.ReadLine();

        if (inPut == "1")
        {
            status.ShowStatus();
        }
        else if (inPut == "2")
        {
            inventory.ShowInventory();
        }
        else if (inPut == "3")
        {
            shop.ShowShop(player, inventory);
        }
        else
        {
            Console.Clear();
            Console.WriteLine("잘못된 입력입니다");
            winput = true;
            home();
        }
    }
}


public class Status
{
    private Program pro;

    public Status(Program program)
    {
        pro = program;
    }

    public void ShowStatus()
    {
        Console.Clear();
        Console.WriteLine("상태 보기");
        Console.WriteLine("");

        int totalAttack = pro.player.Attack + pro.inventory.GetTotalAttack();
        int totalDefense = pro.player.Defense + pro.inventory.GetTotalDefense();

        Console.WriteLine($"Lv. {pro.player.Level.ToString("D2")}");
        Console.WriteLine($"{pro.player.Name} ( {pro.player.Job} )");
        Console.WriteLine($"공격력 : {pro.player.Attack} (+{pro.inventory.GetTotalAttack()})");
        Console.WriteLine($"방어력 : {pro.player.Defense} (+{pro.inventory.GetTotalDefense()})");
        Console.WriteLine($"체 력 : {pro.player.Health}");
        Console.WriteLine($"Gold : {pro.player.Gold} G");
        Console.WriteLine("");
        Console.WriteLine("0. 나가기");
        Console.WriteLine("");
        Console.Write(">>");

        string inPut = Console.ReadLine();
        if (inPut == "0")
        {
            pro.home();
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowStatus();
        }
    }

}


public class Inventory
{
    private List<Item> items = new List<Item>();
    private Program pro;

    public Inventory(Program program)
    {
       pro=program;
    }
    public bool Contains(string itemName)
    {
        return items.Exists(i => i.Name == itemName);
    }

    public void AddItem(Item item)
    {
        var existingItem = items.Find(i => i.Name == item.Name);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            items.Add(item);
        }
    }

    public int GetTotalAttack()
    {
        int total = 0;
        foreach (var item in items)
        {
            if (item.IsEquipped) // 장착된 아이템만 계산
            {
                total += item.Attack;
            }
        }
        return total;
    }

    public int GetTotalDefense()
    {
        int total = 0;
        foreach (var item in items)
        {
            if (item.IsEquipped) // 장착된 아이템만 계산
            {
                total += item.Defense;
            }
        }
        return total;
    }

    public void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

        if (items.Count == 0)
        {
            Console.WriteLine("인벤토리가 비어 있습니다.");
        }
        else
        {
            foreach (var item in items)
            {
                string equippedText = item.IsEquipped ? "[E] " : "";
                Console.WriteLine($"- {equippedText}{item.Name} | {item.Description}");
            }
        }

        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("0. 나가기");
        Console.Write(">>");

        string input = Console.ReadLine();
        if (input == "1")
        {
            ManageEquipment(pro);
        }
        else if (input == "0")
        {
            Console.Clear();
            pro.home();
        }
    }

    private void ManageEquipment(Program pro)
    {
        Console.Clear();
        Console.WriteLine("장착 관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

        for (int i = 0; i < items.Count; i++)
        {
            string equippedText = items[i].IsEquipped ? "[E] " : "";
            Console.WriteLine($"{i + 1}. {equippedText}{items[i].Name} | {items[i].Description}");
        }

        Console.WriteLine("0. 나가기");
        Console.Write(">>");

        string input = Console.ReadLine();
        if (input == "0")
        {
            Console.Clear();
            pro.home();
        }

        if (int.TryParse(input, out int index) && index >= 1 && index <= items.Count)
        {
            items[index - 1].IsEquipped = !items[index - 1].IsEquipped;
            Console.WriteLine(items[index - 1].IsEquipped ? "장착 완료!" : "장착 해제!");

            
            ManageEquipment(pro);
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ManageEquipment(pro);
        }
    }

}


public class Player
{
    public int Level { get; set; }
    public string Name { get; set; }
    public string Job { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Health { get; set; }
    public int Gold { get; set; }
}


public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }
    public bool IsEquipped { get; set; }

    public Item(string name, string description, int attack,int defense,int quantity, int price)
    {
        Name = name;
        Description = description;
        Attack = attack;
        Defense = defense;
        Quantity = quantity;
        Price = price;
        IsEquipped = false;


    }
}


public class Shop
{
    private List<Item> shopItems = new List<Item>();
    private Program pro;


    public Shop(Program program)
    {
        pro = program;
        // 상점 초기 아이템 목록
        shopItems.Add(new Item("수련자 갑옷", "방어력 +5 | 수련에 도움을 주는 갑옷입니다.", 0, 5, 1, 1000));
        shopItems.Add(new Item("무쇠갑옷", "방어력 +9 | 무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, 1, 2000));
        shopItems.Add(new Item("스파르타의 갑옷", "방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, 1, 3500));
        shopItems.Add(new Item("낡은 검", "공격력 +2 | 쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, 1, 600));
        shopItems.Add(new Item("청동 도끼", "공격력 +5 | 어디선가 사용됐던거 같은 도끼입니다.", 5, 0, 1, 1500));
        shopItems.Add(new Item("스파르타의 창", "공격력 +7 | 스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, 1, 2500));
    }

    public void ShowShop(Player player, Inventory inventory)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine($"[보유 골드]");
            Console.WriteLine($"{player.Gold} G\n");

            Console.WriteLine("[아이템 목록]");
            foreach (var item in shopItems)
            {
                string priceText = inventory.Contains(item.Name) ? "구매완료" : $"{item.Price} G";
                Console.WriteLine($"- {item.Name} | {item.Description} | {priceText}");
            }

            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.Clear();
                BuyItem(player, inventory);
            }
            else if (input == "0")
            {
                pro.home();
            }
            else
            {
                Console.WriteLine("올바른 값을 입력해주세요.");
                Console.ReadLine();
            }
        }
    }

    private void BuyItem(Player player, Inventory inventory)
    {

        Console.WriteLine("상점 - 아이템 구매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine($"[보유 골드]");
        Console.WriteLine($"{player.Gold} G\n");

        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < shopItems.Count; i++)
        {
            var item = shopItems[i];
            string priceText = inventory.Contains(item.Name) ? "구매완료" : $"{item.Price} G";
            Console.WriteLine($"{i + 1}. {item.Name} | {item.Description} | {priceText}");
        }

        Console.WriteLine("0. 나가기");
        Console.Write(">> ");
        string input = Console.ReadLine();

        if (input == "0")
        {
            pro.home();
        }

        if (int.TryParse(input, out int index) && index >= 1 && index <= shopItems.Count)
        {
            var selectedItem = shopItems[index - 1];

            if (inventory.Contains(selectedItem.Name))
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
            }
            else if (player.Gold >= selectedItem.Price)
            {
                player.Gold -= selectedItem.Price; // 골드 감소
                inventory.AddItem(selectedItem); // 인벤토리에 추가
                Console.Clear();
                Console.WriteLine("구매를 완료했습니다.");
                BuyItem(player, inventory);
            }
            else
            {
                Console.WriteLine("Gold가 부족합니다.");
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }

        Console.ReadLine();
    }
}