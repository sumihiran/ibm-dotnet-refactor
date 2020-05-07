using Microsoft.Data.Sqlite;

namespace IBM.Products.Models
{
    public static class Helpers
    {
        private const string ConnectionString = @"Data Source=./Products.db;";

        private static string InitScript = @"
            DROP TABLE IF EXISTS `product_option`;
            DROP TABLE IF EXISTS `product`;

            CREATE TABLE `product` (
              `id` varchar(36) DEFAULT NULL,
              `name` varchar(17) DEFAULT NULL,
              `description` varchar(35) DEFAULT NULL,
              `price` decimal(6,2) DEFAULT NULL,
              `delivery_price` decimal(4,2) DEFAULT NULL
            );

            INSERT INTO `product` VALUES 
                ('8f2e9176-35ee-4f0a-ae55-83023d2db1a3','Samsung Galaxy S7','Newest mobile product from Samsung.',1024.99,16.99),
                ('de1287c0-4b15-4a7b-9d8a-dd21b3cafec3','Apple iPhone 6S','Newest mobile product from Apple.',1299.99,15.99);

            CREATE TABLE `product_option` (
              `id` varchar(36) DEFAULT NULL,
              `product_id` varchar(36) DEFAULT NULL,
              `name` varchar(9) DEFAULT NULL,
              `description` varchar(23) DEFAULT NULL
            );

            INSERT INTO `product_option` VALUES 
                ('0643ccf0-ab00-4862-b3c5-40e2731abcc9','8f2e9176-35ee-4f0a-ae55-83023d2db1a3','White','White Samsung Galaxy S7'),
                ('a21d5777-a655-4020-b431-624bb331e9a2','8f2e9176-35ee-4f0a-ae55-83023d2db1a3','Black','Black Samsung Galaxy S7'),
                ('5c2996ab-54ad-4999-92d2-89245682d534','de1287c0-4b15-4a7b-9d8a-dd21b3cafec3','Rose Gold','Gold Apple iPhone 6S'),
                ('9ae6f477-a010-4ec9-b6a8-92a85d6c5f03','de1287c0-4b15-4a7b-9d8a-dd21b3cafec3','White','White Apple iPhone 6S'),
                ('4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef','de1287c0-4b15-4a7b-9d8a-dd21b3cafec3','Black','Black Apple iPhone 6S');
        ";

        public static void Seed()
        {
            var conn = NewConnection();
            conn.Open();
            var migration = new SqliteCommand(InitScript, conn);
            migration.ExecuteNonQuery();
            conn.Close();
        }
        

        public static SqliteConnection NewConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}