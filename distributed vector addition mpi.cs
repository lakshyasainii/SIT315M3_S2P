#include <mpi.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#define VEC_SIZE 100000
int main(int argc, char** argv)
{
    // Initialize MPI
    MPI_Init(&argc, &argv);
    int rank, size;
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);
    // Allocate memory for
    vectors int* vec1 = NULL;
    int* vec2 = NULL; int* vec3 =
    NULL;
    // Master process initializes vectors if
    (rank == 0) {
        vec1 =
    (int*)malloc(sizeof(int) * VEC_SIZE); vec2
    = (int*)malloc(sizeof(int) * VEC_SIZE);
        vec3 = (int*)malloc(sizeof(int) * VEC_SIZE);
        // Generate random values for
        vectors srand(time(NULL));
        for (int i = 0; i < VEC_SIZE; i++)
        {
            vec1[i] = rand() % 100; vec2[i]
            = rand() % 100;
        }
    }
    // Scatter the vectors to all processes
    MPI_Scatter(vec1, VEC_SIZE / size, MPI_INT, vec1, VEC_SIZE / size, MPI_INT,
    0, MPI_COMM_WORLD);
    MPI_Scatter(vec2, VEC_SIZE / size, MPI_INT, vec2, VEC_SIZE / size, MPI_INT,
    0, MPI_COMM_WORLD);
    // Perform local vector addition for
    (int i = 0; i < VEC_SIZE / size; i++) {
        vec3[i] = vec1[i] + vec2[i];
    }
    // Gather the results back to the master process
    MPI_Gather(vec3, VEC_SIZE / size, MPI_INT, vec3, VEC_SIZE / size, MPI_INT,
    0, MPI_COMM_WORLD);
    // Calculate the total sum using
    MPI_Reduce int total_sum = 0; for (int
    i = 0; i < VEC_SIZE; i++)
    {
        total_sum
    += vec3[i];
    }
    int global_sum;
    MPI_Reduce(&total_sum, &global_sum, 1, MPI_INT, MPI_SUM, 0, MPI_COMM_WORLD);
    // Print the total sum on the master process if
    (rank == 0) {
        printf("Total sum of vectors:
    % d\n", global_sum"); }
    // Clean up allocated memory on the master
    process if (rank == 0)
        {
            free(vec1);
            free(vec2); free(vec3);
        }
        // Finalize MPI
        MPI_Finalize(); return
        0;
    }