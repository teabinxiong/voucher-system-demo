# voucher-system-demo
Voucher System written in .NET Core, Redis, and Quatz.NET scheduler library, demonstrated how the Voucher System works where in high traffic situation.

## Author
Tea Bin Xiong

## Architecture Diagram
![image](./img/voucher-system.PNG)


## Explanations
1) The benefit of this architecture is that the vouchers won't be over-distributed as we pre-generate them, especially during high peak seasons.
2) The importance of using Redis as our voucher store is to prevent two or more people from gaining access to the same voucher. The logic works such that only a single thread can access the Redis record at a time, and thus solving the issue of multiple people accessing same voucher.

## Modules
1) File Processor service
File processor service read the voucher files from the target directory every 2 minutes.

2) Consumer Simulation Service 
Background services that act a client grabbing for the voucher every seconds.



## Repository URL
[cdr-inserter-demo](https://github.com/teabinxiong/voucher-system-demo/)
