To Run Project 

1 Change "Server" name in "ConnectionStrings" from appsetting.json file as per your database server name
2 Then run command "update-database", in Package Manager Console 
3 Add Required nuget packages
3 Project is Ready to Execute now 

Project Flow 
1 First do Register and Login user with role Admin
2 Authories use with jwt token give in login responce 
3 Now First Create New Category 
4 Now Add new Vendor  
4 Then Add Product with that CategoryId
5 Now Purchaser that Product From Vendor 
6 Update Status of Purchase Order to recive when order is recived to you