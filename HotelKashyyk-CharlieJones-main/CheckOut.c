#include <ctype.h>
#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

void checkOut(void);

char userNames[6][30] = {"-1","-1","-1","-1","-1","-1"};
char dateOfBirth[6][8];
char boardType[6][2];
float boardRates[3] = {20, 15, 5};
int stayLength[6];
int dailyNewspaper[6];
int numOfAdults[6];
int numOfChildren[6];
int availableRooms[6] = {0, 0, 0, 0, 0, 0};
float roomRates[6] = {100, 100, 85, 75,75, 50};
char bookingIds[6][40] = {""};

int main(void) {
    // CHECK IN CODE TO BE REMOVED LATER
    int ifAvailableRooms = 0;
    int customerNumber = 0;
    int roomChoice = 0;
    char bookingId[25] = "";
    bool bookingIdUsed = true;
    bool lastName = false;
    int lastNameLetter = 0;
    char lastNameNumberStr[15];
    char bookingIdTemp[30] = "";

    for (int i = 0; i < 6; i++) {
        if (availableRooms[i] == 1) {
            ifAvailableRooms = 1;
            break;
        }
    }

    if (ifAvailableRooms == 1) {
        printf("Sadly there are no rooms available\n");
        return 0;
    }

    for (int i = 0; i < 6; i++) {
        if (strcmp(userNames[i], "-1") == 0) {
            customerNumber = i;
            break;
        }
    }

    printf("Enter your first and last name, with a space between the two: ");
    fgets(userNames[customerNumber], 30, stdin);
    userNames[customerNumber][strcspn(userNames[customerNumber], "\n")] = '\0';

    printf("Please enter your date of birth in the ddmmyyyy format: ");
    scanf("%s", dateOfBirth[customerNumber]);
    fflush(stdin);

    printf("Please enter the number of under-16's: ");
    scanf("%d", &numOfChildren[customerNumber]);
    fflush(stdin);

    printf("Please enter the number of adults: ");
    scanf("%d", &numOfAdults[customerNumber]);
    fflush(stdin);

    printf("Please enter FB for full board,\nHB for half board\nor BB for bed and breakfast: ");
    scanf("%s", boardType[customerNumber]);
    fflush(stdin);

    printf("How long will you stay: ");
    scanf("%d", &stayLength[customerNumber]);
    fflush(stdin);

    printf("Do you want a newspaper? Enter 1 for yes, 0 for no: ");
    scanf("%d", &dailyNewspaper[customerNumber]);
    fflush(stdin);

    printf("The rooms that are available include:\n");
    for (int i = 0; i < 6; i++) {
        if (availableRooms[i] == 0) {
            printf("Room %d - Price: %.2f\n", i + 1, roomRates[i]);
        }
    }

    printf("Enter the number of the room you want: ");
    scanf("%d", &roomChoice);
    roomChoice -= 1;
    availableRooms[roomChoice] = 1;
    fflush(stdin);

    availableRooms[customerNumber] = roomChoice;

    for (int i = 0; i < strlen(userNames[customerNumber]); i++) {
        if (isspace(userNames[customerNumber][i])) {
            lastName = true;
            continue;
        }
        if (lastName) {
            bookingId[lastNameLetter++] = userNames[customerNumber][i];
        }
    }
    bookingId[lastNameLetter] = '\0';

    srand(time(0));
    while (bookingIdUsed) {
        bookingIdTemp[0] = '\0';
        int bookingIdNum = rand() % (9999 - 1000 + 1) + 1000;
        sprintf(lastNameNumberStr, "%d", bookingIdNum);

        strcat(bookingIdTemp, bookingId);
        strcat(bookingIdTemp, lastNameNumberStr);

        bookingIdUsed = false;
        for (int i = 0; i < 6; i++) {
            if (strcmp(bookingIdTemp, bookingIds[i]) == 0) {
                bookingIdUsed = true;
                break;
            }
        }
    }
    strcpy(bookingIds[customerNumber], bookingIdTemp);
    printf("You're booked! Your ID is %s\n", bookingIds[customerNumber]);

    checkOut();
    return 0;
}

void checkOut() {
    char id[40];
    int index = -1;
    float totalBoardCost = 0, totalBill = 0 , discounts = 0;
    float totalRoomCost = 0;
    char birthYearStr[5];
    int birthYear;
    int age;

    printf("\n===================[ CHECK OUT MENU ]===================\n");

    while (index == -1) {
        fflush(stdin);
        printf("Enter your booking ID: ");
        scanf("%s", id);
        for(int i = 0; i < 6; i++) {
            if(strcmp(bookingIds[i], id) == 0) {
                index = i;
                break;
            }
        }
        if (index == -1) {
            printf("Invalid booking ID. Please try again.\n");
        }
    }

    // Board Cost Calculation
    if(strcmp(boardType[index], "FB") == 0) {
        totalBoardCost += boardRates[0] * stayLength[index] * numOfAdults[index];
        if(numOfChildren[index] > 0) {
            totalBoardCost += (boardRates[0] * stayLength[index] * numOfChildren[index]) / 2;
            discounts += (boardRates[0] * stayLength[index] * numOfChildren[index]) / 2;
        }
    } else if(strcmp(boardType[index], "HB") == 0) {
        totalBoardCost += boardRates[1] * stayLength[index] * numOfAdults[index];
        if(numOfChildren[index] > 0) {
            totalBoardCost += (boardRates[1] * stayLength[index] * numOfChildren[index]) / 2;
            discounts += (boardRates[1] * stayLength[index] * numOfChildren[index]) / 2;
        }
    } else if(strcmp(boardType[index], "BB") == 0) {
        totalBoardCost += boardRates[2] * stayLength[index] * numOfAdults[index];
        if(numOfChildren[index] > 0) {
            totalBoardCost += (boardRates[2] * stayLength[index] * numOfChildren[index]) / 2;
            discounts += (boardRates[2] * stayLength[index] * numOfChildren[index]) / 2;
        }
    }

    // Room Cost Calculation
    if (availableRooms[index] >= 1 && availableRooms[index] <= 6) {
        totalRoomCost = roomRates[availableRooms[index] - 1] * stayLength[index];
    } else {
        printf("ERROR: Invalid room choice.");
    }

    // Add newspaper charge
    if(dailyNewspaper[index] == 1) {
        totalBill += 5.5;
    }

    // Age Discount Check
    strncpy(birthYearStr, dateOfBirth[index] + 4, 4);
    birthYearStr[4] = '\0';
    birthYear = atoi(birthYearStr);
    age = 2024 - birthYear;

    // Only add discounts if applicable
    if (age > 65) {
        discounts += totalRoomCost * 0.1;
        totalRoomCost *= 0.9;
    }

    // Display Final Bill
    printf("\n========================================================\n");
    printf("                     FINAL BILL SUMMARY                  \n");
    printf("========================================================\n");
    printf("User Name      : %s\n", userNames[index]);
    printf("Booking ID     : %s\n", bookingIds[index]);
    printf("\n----------------------- Charges ------------------------\n");
    printf("Room Costs     : £%.2f\n", totalRoomCost);
    printf("Board Costs    : £%.2f\n", totalBoardCost);
    if (dailyNewspaper[index] == 1) {
        printf("Daily Newspaper: £%.2f\n", 5.5);
    }
    if (discounts > 0) {
        printf("Discounts      : £%.2f\n", discounts);
    }
    printf("\n----------------------- Total --------------------------\n");
    totalBill += totalRoomCost + totalBoardCost;
    printf("Total Bill      : £%.2f\n", totalBill);
    printf("========================================================\n");

    strcpy(userNames[index], "-1");
    strcpy(dateOfBirth[index], "");
    strcpy(boardType[index], "");
    dailyNewspaper[index] = 0;
    numOfAdults[index] = 0;
    numOfChildren[index] = 0;
    availableRooms[index] = 0;
    strcpy(bookingIds[index], "");
}