#include <mpi.h>
#include <stdio.h>
#include <stdlib.h> // Added for dynamic memory allocation
#include <time.h>   // Added for random number generation

#define ARRAY_SIZE 100 // Define the size of array v3

int main(int argc, char **argv)
{
    int rank, size;
    int *v3; // Changed to dynamically allocate memory for array v3
    int total_sum = 0; 

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);

    if (rank == 0)
    {
        v3 = (int *)malloc(ARRAY_SIZE * sizeof(int)); // Allocate memory for v3 only in rank 0
        srand(time(NULL)); // Seed for random number generation
        
        // Initialize v3 with random values
        for (int i = 0; i < ARRAY_SIZE; i++)
        {
            v3[i] = rand() % 100; // Generate random numbers between 0 and 99
        }
    }
    else
    {
        v3 = NULL; // Other ranks will not use v3
    }

    // Broadcast the array v3 from rank 0 to all other processes
    MPI_Bcast(v3, ARRAY_SIZE, MPI_INT, 0, MPI_COMM_WORLD);

    // Calculate partial sum of elements in v3 for each process
    int partial_sum = 0;
    for (int i = 0; i < ARRAY_SIZE; i++)
    {
        partial_sum += v3[i];
    }

    // Reduce partial sums from all processes to calculate total sum
    MPI_Reduce(&partial_sum, &total_sum, 1, MPI_INT, MPI_SUM, 0, MPI_COMM_WORLD);

    // Print total sum from rank 0
    if (rank == 0)
    {
        printf("Total sum of all elements in v3: %d\n", total_sum);
        free(v3); // Free dynamically allocated memory
    }

    MPI_Finalize();
    return 0;
}
