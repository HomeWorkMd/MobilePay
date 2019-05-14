# MobilePay transaction fee Calculator 

Application calculates transaction fees for merchants based on Transactions supplied (by default via **'transactions.txt'** file)

## Usage

 - Start the application
   - Press ENTER to use default 'transactions.txt' file as input
    - or enter a custom input file name and press ENTER
    - or input file name can be specified as command line argument like this:  `MobilePay.exe "c:\temp\transactions.txt"`

## Result

Application prints the results out into the console
using the following format `$"{Date:yyyy-MM-dd} {Merchant.OriginalName} {Fee:0.00}"`

In case of Invalid Input data - Error Details will be printed out to the output
and application will continue processing transactions


