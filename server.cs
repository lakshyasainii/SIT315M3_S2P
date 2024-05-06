#include <mpi.h>
#include <stdio.h>
#include <string.h>

int main(int argc, char** argv) {
    int rank, size; 
    char msg[256]; // Assuming messages won't exceed 256 characters
    
    MPI_Init(&argc, &argv);
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);
    
    if (rank != 0) {
        fprintf(stderr, "Error: Server should be run with rank 0.\n");
        MPI_Abort(MPI_COMM_WORLD, 1);
    }
    
    printf("Server process started.\n");
    
    while (1) {
        MPI_Recv(msg, 256, MPI_CHAR, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, MPI_STATUS_IGNORE); 
        int source = MPI_STATUS.MPI_SOURCE; 
        int tag = MPI_STATUS.MPI_TAG;
        
        // Process message based on the tag
        if (tag == 1) { // Tag 1: Client requests a service
            // Assuming the message contains a service request (e.g., "SERVICE:123")
            // Extract the service ID and process the request
            int serviceID;
            sscanf(msg, "SERVICE:%d", &serviceID);
            
            // Process the service request (e.g., call a function to handle the service)
            printf("Server received service request ID %d from client %d.\n", serviceID, source);
            
            // Process the request and send back the response
            sprintf(msg, "Response to service ID %d", serviceID);
            MPI_Send(msg, strlen(msg) + 1, MPI_CHAR, source, 0, MPI_COMM_WORLD);
        } else if (tag == 2) { // Tag 2: Client sends a message
            // Process the message received from the client
            printf("Server received message '%s' from client %d.\n", msg, source);
            
            // Process the message and send back an acknowledgment
            MPI_Send("Message received", strlen("Message received") + 1, MPI_CHAR, source, 0, MPI_COMM_WORLD);
        } else {
            printf("Server received an unknown message from client %d.\n", source);
        }
    }
    
    MPI_Finalize();
    return 0;
}
