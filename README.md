# Отчет о реализации проекта "Тяжелая муравьиная жизнь"  
**Язык разработки**: C# (.NET 7.0)  
**Тип приложения**: Консольное  
**Автор**: Лутченко А.М.   
**Группа**: БСБО-09-21

---

## 1. Архитектурный дизайн  

![ClassDiagram1](https://github.com/user-attachments/assets/29697823-bc67-4f53-80b7-e735ddf1d690)

### 1.1. Ключевые компоненты системы  
#### Класс `Game` (Singleton)  
- **Ответственность**:  
  - Управление игровым циклом (15 дней).  
  - Инициализация куч ресурсов (`ResourcePile`) и колоний (`Colony`).  
  - Обработка события легендарного мифического муравья (`LegendaryMythicalAnt`).  

- **Особенности реализации**:  
  ```csharp
  // Пример инициализации куч ресурсов
  _resourcePiles.Add(new ResourcePile(1, new Dictionary<string, int> {
      { "веточка", 39 }, { "листик", 35 }, { "росинка", 34 }
  }));
  ```  
  - Использование паттерна Singleton гарантирует единую точку доступа к игровому состоянию.  
  - В методе `RunSimulation` реализована поэтапная обработка дней с проверкой условий завершения (засуха, уничтожение колоний, истощение ресурсов).  

---

#### Класс `Colony`  
- **Ответственность**:  
  - Управление популяцией (рабочие, воины, особые насекомые).  
  - Сбор ресурсов и боевые взаимодействия.  
  - Взаимодействие с королевой (`Queen`).  

- **Ключевые методы**:  
  - **`ProcessDay()`**:  
    ```csharp
    // Распределение рабочих по случайным кучам
    foreach (var worker in Workers.Where(w => w.IsAlive)) {
        var randomPile = availablePiles[random.Next(availablePiles.Count)];
        var gathered = worker.GatherResources(randomPile.Resources, enemies);
        // Логика добавления ресурсов...
    }
    ```  
    - Рабочие муравьи собирают ресурсы с учетом их специализации (например, `AdvancedPickpocketWorker` крадет ресурсы у врагов).  

  - **Обработка атак**:  
    ```csharp
    // Атака воинов с учетом типа (например, AdvancedHunterWarrior)
    var validTargets = allEnemyInsects.Where(i => i.IsInvulnerable).ToList();
    warrior.Attack(validTargets);
    ```  
    - Воины выбирают цели в зависимости от своих характеристик (например, охотники атакуют только неуязвимых).  

---

### 1.2. Особые насекомые  
#### Класс `Dragonfly`  
- **Механики**:  
  - Неуязвимость к атакам (`IsInvulnerable = true`).  
  - Игнорирование негативных эффектов для колонии через метод `ApplyColonyEffect`:  
    ```csharp
    public void ApplyColonyEffect(Colony colony) {
        colony.IgnoreNegativeEffects = true;
    }
    ```  

#### Класс `Bumblebee`  
- **Механики**:  
  - Атака 3 целей с тремя укусами каждой:  
    ```csharp
    for (int i = 0; i < 3; i++) {
        int actualDamage = Math.Max(0, Damage - target.Defense);
        target.TakeDamage(actualDamage);
    }
    ```  
  - Уменьшенная защита (`Defense /= 2`).  

---

### 1.3. Дополнительное задание: `LegendaryMythicalAnt`  
- **Реализация**:  
  - Однократная активация на 1-й день игры:  
    ```csharp
    if (!hasActivated) {
        int targetColonyIndex = random.Next(colonies.Count);
        colonies[targetColonyIndex].IgnoreNegativeEffects = true;
        // Уничтожение других колоний...
    }
    ```  
  - Уничтожает все колонии, кроме случайно выбранной, если они не защищены эффектом `IgnoreNegativeEffects`.  

---

## 2. Детали реализации  
### 2.1. Система ресурсов  
- **Типы ресурсов**: Веточка, листик, росинка, камушек.  
- **Распределение**:  
  - Рабочие выбирают кучи случайным образом.  
  - Продвинутый карманник (`AdvancedPickpocketWorker`) крадет ресурсы у врагов при их отсутствии в куче.  

### 2.2. Боевая система  
- **Формула урона**:  
  ```csharp
  int actualDamage = Math.Max(0, Damage - target.Defense);
  ```  
- **Особые случаи**:  
  - Обычный крепкий воин (`RegularToughWarrior`) уменьшает получаемый урон на 50%.  
  - Шмель (`Bumblebee`) игнорирует защиту при атаке.  

---

## 3. Примеры работы системы  
### Инициализация колоний  
```csharp
private void InitializeRedColony()
{
    var queen = new Queen("Маргрете", 22, 7, 20, 2, 4, 4);
    var colony = new Colony("красные", queen);

    // Добавление рабочих
    for (int i = 0; i < 4; i++)
        colony.AddWorker(new EliteWorker());
    for (int i = 0; i < 4; i++)
        colony.AddWorker(new AdvancedWorker());
    for (int i = 0; i < 5; i++)
        colony.AddWorker(new AdvancedPickpocketWorker());

    // Добавление воинов
    for (int i = 0; i < 3; i++)
        colony.AddWarrior(new LegendaryWarrior());
    for (int i = 0; i < 2; i++)
        colony.AddWarrior(new SeniorWarrior());
    for (int i = 0; i < 3; i++)
        colony.AddWarrior(new RegularToughWarrior());

    // Добавление особого насекомого
    colony.AddSpecialInsect(new Dragonfly());

    _colonies.Add(colony);
}
```  

### Вывод статуса колонии  
```csharp
public void DisplayInfo() {
    Console.WriteLine($"Колония \"{Name}\"");
    Console.WriteLine($"Ресурсы: {string.Join(", ", Resources.Select(r => $"{r.Key}={r.Value}"))}");
    // Группировка муравьев по типам...
}
```  

### Пример вывода в консоли

![image](https://github.com/user-attachments/assets/280caf4a-8d4e-4e65-b289-7c1461c059ec)

![image](https://github.com/user-attachments/assets/eb13465e-ac1c-4f5a-a5f0-45cc2d33d203)

![image](https://github.com/user-attachments/assets/44f46c77-67ca-4d9c-9fef-4d88c9ff3617)
