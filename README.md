# EnergyHelpline QuotesTool
Other than the specification provided in the requirements specification document, i have made some assumptions in executing this assessment. And these are as follows:

1. Energy plans with no expiration date on the initial unit rate, will always use the initial unit rate only to compute annual cost of a plan.

2. Energy plans with expired initial rate (when the date of the Initial Rate expiration has lapsed - past the date the quote was generated) will default to use the final unit rate only (as initial rate is not applicable to that quote because its expired).

3. I have made allowances for leap year in the calculations. Actually i have statically used the 365 days approach as indicated in the requirements but i have also made allowances for leap years (commented out in the code, uncommenting this will enable leap year suport if desired).
 
4. I have persisted the collection of energy plans in an in-memory collection. Ideally this will be persisted in some sort of data store and asynchronously retrieved on application startup, but i felt that was an overkill for this use case.
 
5. I have not implemented logging or comprehensive error handling. I have intentionally decided not to prioritise these.
 
6. I have added tests for the core functionalities of the application.

Thank you for taking time to consider my application.

Regards

Kay
