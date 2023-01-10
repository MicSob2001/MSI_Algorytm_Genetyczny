
using System.Numerics;

class Example {
	
	// Target string to be generated
	public const string TARGET = "I love programming";
	// Valid Genes
	const string GENES = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890, .-;:_!\"#%&/()=?@${[]}";
	

	// Function to generate random numbers in given range
	public static int random_num(int start, int end)
	{
		Random random = new Random();
		int range = (end - start) + 1;
		int random_int = start + (random.Next() % range);
		return random_int;
	}

	// Create random genes for mutation
	public static char mutated_genes()
	{
		int len = GENES.Length;
		int r = random_num(0, len - 1);
		return GENES[r];
	}

	// create chromosome or string of genes
	public static string create_gnome()
	{
		int len = TARGET.Length;
		string gnome = "";
		for (int i = 0; i < len; i++)
			gnome += mutated_genes();
		return gnome;
	}
}

// Class representing individual in population
class Individual : IComparable<Individual>
{

	public string chromosome;
	public int fitness;
	public Individual(string chromosome)
    {
		this.chromosome = chromosome;
		fitness = cal_fitness();
	}

    public int CompareTo(Individual other)
    {
        return fitness.CompareTo(other.fitness);
    }

    // Perform mating and produce new offspring
    public Individual mate(Individual par2)
    {
		// chromosome for offspring
		string child_chromosome = "";

		int len = chromosome.Length;
		for (int i = 0; i < len; i++)
		{
			// random probability
			float p = Example.random_num(0, 100) / 100;

			// if prob is less than 0.45, insert gene
			// from parent 1
			if (p < 0.45)
				child_chromosome += chromosome[i];

			// if prob is between 0.45 and 0.90, insert
			// gene from parent 2
			else if (p < 0.90)
				child_chromosome += par2.chromosome[i];

			// otherwise insert random gene(mutate),
			// for maintaining diversity
			else
				child_chromosome += Example.mutated_genes();
		}

		// create new Individual(offspring) using
		// generated chromosome for offspring
		return new Individual(child_chromosome);
	}
	// Calculate fitness score, it is the number of
	// characters in string which differ from target
	// string.
	int cal_fitness()
    {
		int len = Example.TARGET.Length;
		int fitness = 0;
		for (int i = 0; i < len; i++)
		{
			if (chromosome[i] != Example.TARGET[i])
				fitness++;
		}
		return fitness;
	}
}

// Overloading < operator


class Program
{
	const int POPULATION_SIZE = 100;
	// Driver code
	//bool operator <(const Individual &ind1, const Individual &ind2)
	//{
	//	return ind1.fitness < ind2.fitness;
	//}

	public static void Main(string[] args)
	{
		Random random = new Random();

		// current generation
		int generation = 0;
        bool found = false;


        List<Individual> population = new List<Individual>();
        // create initial population
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            string gnome = Example.create_gnome();
            population.Add(new Individual(gnome));
        }

        while (!found)
		{
			// sort the population in increasing order of fitness score
			population.Sort();

			// if the individual having lowest fitness score ie.
			// 0 then we know that we have reached to the target
			// and break the loop
			if (population[0].fitness <= 0)
			{
				found = true;
				break;
			}

			// Otherwise generate new offsprings for new generation
			List<Individual> new_generation = new List<Individual>();

			// Perform Elitism, that mean 10% of fittest population
			// goes to the next generation
			int s = (10 * POPULATION_SIZE) / 100;
			for (int i = 0; i < s; i++)
				new_generation.Add(population[i]);

			// From 50% of fittest population, Individuals
			// will mate to produce offspring
			s = (90 * POPULATION_SIZE) / 100;
			for (int i = 0; i < s; i++)
			{
				int len = population.Count;
				int r = Example.random_num(0, 50);
				Individual parent1 = population[r];
				r = Example.random_num(0, 50);
				Individual parent2 = population[r];
				Individual offspring = parent1.mate(parent2);
				new_generation.Add(offspring);
			}
			population = new_generation;
			Console.Write("Generation: " + generation + "\t");
			Console.Write("String: " + population[0].chromosome + "\t");
			Console.Write("Fitness: " + population[0].fitness + "\n");

			generation++;
		}
		Console.Write("Generation: " + generation + "\t");
		Console.Write("String: " + population[0].chromosome + "\t");
		Console.Write("Fitness: " + population[0].fitness + "\n");
	}
}
