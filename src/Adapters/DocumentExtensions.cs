namespace Bookfy.Publishers.Api.Adapters
{
    public static class DocumentExtensions
    {
        public static bool ValidDocument(this string cpfCnpj) 
            => ValidCpf(cpfCnpj) || ValidCnpj(cpfCnpj);

        public static bool ValidCpf(this string cpf)
        {
            int[] firstMultiples = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] secondMultiples = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            cpf = ClearDocument(cpf);
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf[..9];
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * firstMultiples[i];

            var rest = sum % 11;
            rest = rest < 2 
                ? 0
                : 11 - rest;

            var digit = rest.ToString();
            tempCpf += digit;

            sum = 0;
            for (var i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * secondMultiples[i];

            rest = sum % 11;
            rest = rest < 2 
                ? 0 
                : 11 - rest;

            digit += rest.ToString();

            return cpf.EndsWith(digit);
        }

        public static bool ValidCnpj(this string cnpj)
        {
            int[] firstMultiples = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] secondMultiples = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            cnpj = ClearDocument(cnpj);

            if (cnpj.Length != 14)
                return false;

            var tempCnpj = cnpj[..12];
            var sum = 0;

            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * firstMultiples[i];

            int rest = sum % 11;
            rest = rest < 2 
                ? 0 
                : 11 - rest;

            var digit = rest.ToString();
            tempCnpj += digit;

            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * secondMultiples[i];

            rest = sum % 11;
            rest = rest < 2 
                ? 0 
                : 11 - rest;

            digit += rest.ToString();

            return cnpj.EndsWith(digit);
        }

        public static string ClearDocument(string document)
            => document.Trim()
                .Replace(".", "")
                .Replace("-", "")
                .Replace("/", "");
    }
}