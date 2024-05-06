#include <string.h>
int main(int argc, char** argv)
{
    int rank, size; char
msg[256];
    // Initialize MPI
    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);
    if (rank == 0)
    {
        // Error handling: Client should not be run with rank 0
        fprintf(stderr, "Error: Client should not be run with rank 0.\n");
        MPI_Abort(MPI_COMM_WORLD, 1);
    }
    // Create a message to send from the client to the server
    sprintf(msg, "Hello from Client %d", rank);
    // Send the message to the server (rank 0) with tag 2
    MPI_Send(msg, strlen(msg) + 1, MPI_CHAR, 0, 2, MPI_COMM_WORLD);
    printf("Client %d sent a message to the server.\n", rank);
    // Simulate sending a service request if the client has rank 1
    if (rank == 1)
    {
        // Create a service request
        message sprintf(msg, "SERVICE:%d",
123);
        // Send the service request to the server (rank 0) with tag 1
        MPI_Send(msg, strlen(msg) + 1, MPI_CHAR, 0, 1, MPI_COMM_WORLD);
        printf("Client %d sent a service request to the server.\n", rank);
    }
}