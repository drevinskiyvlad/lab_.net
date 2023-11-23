using Domain.Logic;
using Domain.Models.enums;
using Domain.Models;
using System;
using System.Linq;

class Program
{
    private static readonly SimpleTaskPlanner TaskPlanner = new SimpleTaskPlanner(new FileWorkItemsRepository());

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Task Planner App!");

        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine()?.ToUpper();

            switch (choice)
            {
                case "A":
                    AddWorkItem();
                    break;
                case "B":
                    BuildPlan();
                    break;
                case "M":
                    MarkAsCompleted();
                    break;
                case "R":
                    RemoveWorkItem();
                    break;
                case "Q":
                    Console.WriteLine("Exiting the application. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("\nChoose an operation:");
        Console.WriteLine("[A]dd work item");
        Console.WriteLine("[B]uild a plan");
        Console.WriteLine("[M]ark work item as completed");
        Console.WriteLine("[R]emove a work item");
        Console.WriteLine("[Q]uit the app");
        Console.Write("Enter your choice: ");
    }

    private static void AddWorkItem()
    {
        Console.WriteLine("Adding a new work item...");

        // Gather information for creating a new work item
        var title = ReadNonEmptyInput("Enter the title of the work item: ");
        var description = ReadNonEmptyInput("Enter the description of the work item: ");
        var dueDate = ReadDateTimeInput("Enter the due date (dd.MM.yyyy): ");
        var priority = ReadEnumInput<Priority>("Enter the priority (None, Low, Medium, High, Urgent): ");
        var complexity = ReadEnumInput<Complexity>("Enter the complexity (None, Minutes, Hours, Days, Weeks): ");

        var newWorkItem = new WorkItem
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            Priority = priority,
            Complexity = complexity,
            CreationDate = DateTime.Now,
            IsCompleted = false
        };

        TaskPlanner.AddWorkItem(newWorkItem);

        Console.WriteLine("Work item added successfully!");
    }

    private static void BuildPlan()
    {
        Console.WriteLine("Building a plan...");

        var plannedItems = TaskPlanner.CreatePlan();

        if (plannedItems.Any())
        {
            Console.WriteLine("Planned Work Items:");
            foreach (var item in plannedItems)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("No work items to plan.");
        }
    }

    private static void MarkAsCompleted()
    {
        Console.WriteLine("Marking a work item as completed...");

        var id = ReadGuidInput("Enter the ID of the work item to mark as completed: ");

        if (TaskPlanner.MarkAsCompleted(id))
        {
            Console.WriteLine("Work item marked as completed!");
        }
        else
        {
            Console.WriteLine("Work item not found. Please check the ID.");
        }
    }

    private static void RemoveWorkItem()
    {
        Console.WriteLine("Removing a work item...");

        var id = ReadGuidInput("Enter the ID of the work item to remove: ");

        if (TaskPlanner.RemoveWorkItem(id))
        {
            Console.WriteLine("Work item removed successfully!");
        }
        else
        {
            Console.WriteLine("Work item not found. Please check the ID.");
        }
    }


    private static string ReadNonEmptyInput(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();
        } while (string.IsNullOrEmpty(input));
        return input;
    }

    private static DateTime ReadDateTimeInput(string prompt)
    {
        DateTime dueDate;
        while (true)
        {
            if (DateTime.TryParseExact(ReadNonEmptyInput(prompt), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dueDate))
            {
                return dueDate;
            }
            Console.WriteLine("Invalid date format. Please use the format dd.MM.yyyy.");
        }
    }

    private static TEnum ReadEnumInput<TEnum>(string prompt) where TEnum : struct
    {
        TEnum result;
        while (true)
        {
            if (Enum.TryParse(ReadNonEmptyInput(prompt), true, out result) && Enum.IsDefined(typeof(TEnum), result))
            {
                return result;
            }
            Console.WriteLine($"Invalid input. Please enter a valid {typeof(TEnum).Name}.");
        }
    }

    private static Guid ReadGuidInput(string prompt)
    {
        Guid id;
        while (true)
        {
            if (Guid.TryParse(ReadNonEmptyInput(prompt), out id))
            {
                return id;
            }
            Console.WriteLine("Invalid ID format. Please enter a valid GUID.");
        }
    }
}
