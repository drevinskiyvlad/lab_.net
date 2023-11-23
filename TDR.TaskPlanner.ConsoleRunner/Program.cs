using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Logic;
using Domain.Models;
using Domain.Models.enums;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Task Planner!");

        // Accept user input for WorkItems
        var workItems = GetWorkItemsFromUser();

        // Create an instance of SimpleTaskPlanner and order the WorkItems
        var planner = new SimpleTaskPlanner();
        var orderedItems = planner.CreatePlan(workItems);

        // Display the ordered WorkItems
        Console.WriteLine("\nOrdered WorkItems:");
        foreach (var item in orderedItems)
        {
            Console.WriteLine(item.ToString());
        }
    }

    private static WorkItem[] GetWorkItemsFromUser()
    {
        Console.Write("\nEnter the number of work items: ");
        if (!int.TryParse(Console.ReadLine(), out int itemCount) || itemCount <= 0)
        {
            Console.WriteLine("Invalid input. Exiting program.");
            Environment.Exit(1);
        }

        var workItems = new WorkItem[itemCount];

        for (var i = 0; i < itemCount; i++)
        {
            Console.WriteLine($"\nWorkItem {i + 1}:");

            Console.Write("Title: ");
            var title = Console.ReadLine();

            Console.Write("Due Date (dd.MM.yyyy): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var dueDate))
            {
                Console.WriteLine("Invalid date format. Exiting program.");
                Environment.Exit(1);
            }

            Console.Write("Priority (None, Low, Medium, High, Urgent): ");
            if (!Enum.TryParse<Priority>(Console.ReadLine(), true, out var priority))
            {
                Console.WriteLine("Invalid priority. Exiting program.");
                Environment.Exit(1);
            }

            Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
            if (!Enum.TryParse<Complexity>(Console.ReadLine(), true, out var complexity))
            {
                Console.WriteLine("Invalid complexity. Exiting program.");
                Environment.Exit(1);
            }

            workItems[i] = new WorkItem
            {
                Title = title,
                DueDate = dueDate,
                Priority = priority,
                Complexity = complexity
            };
        }

        return workItems;
    }
}

