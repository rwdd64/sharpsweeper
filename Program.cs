using System;

namespace CampoMinado
{
    class Program
    {
        static void Main(string[] args)
        {
            const int SIZE = 5; // Tamanho do lado do tabuleiro
            const int QTD_MINAS = 5; // QTD_MINAS < SIZE*SIZE!!
                                     // Se não o jogo fica em loop infinito
                                     // Caso QTD_MINAS = SIZE*SIZE, não haverá
                                     // espaços livres

            int[,] board = new int[SIZE, SIZE]; // Marca os locais de bombas; "mapa"
            char[,] display = new char[SIZE, SIZE]; // Tabuleiro que será mostrado ao jogador

            int descobertos = 0; // Espaços do tabuleiro já descobertos

            // Linha e coluna escolhidas pelo usuário
            int choice_l, choice_c; // l -> Linha
                                    // c -> Coluna

            bool finished = false;
            bool won = false;

            // Colocar as minas aleatoriamente
            {
                Random rand = new Random();
                int minas_colocadas = 0;

                while (minas_colocadas < QTD_MINAS) {
                    int l = rand.Next(0, SIZE);
                    int c = rand.Next(0, SIZE);

                    // Se não houver uma mina
                    if (board[l, c] != 1) {
                        // Setar a mina
                        board[l, c] = 1;
                        minas_colocadas++;
                    }
                    // Se já houver mina, ignora e repete o sorteio
                }
            }

            // Resetar display
            for (int l = 0; l < SIZE; ++l)
            {
                for (int c = 0; c < SIZE; ++c)
                {
                    display[l, c] = '_';
                }
            }

            // Game Loop
            while (!finished)
            {
                Console.Clear();

                // Exibir display
                for (int l = 0; l < SIZE; ++l)
                {
                    for (int c = 0; c < SIZE; ++c)
                    {
                        Console.Write("{0} ", display[l, c]);
                    }
                    Console.WriteLine();
                }

                /* 
                 * HACK ABAIXO
                 * DESCOMENTE PARA HABILITÁ-LO
                 *
                // Exibir mapa (hack) ;)
                for (int l = 0; l < SIZE; ++l)
                {
                    for (int c = 0; c < SIZE; ++c)
                    {
                        Console.Write("{0} ", board[l, c]);
                    }
                    Console.WriteLine();
                }
                */

                // Pegar linha e coluna do jogador
                do
                {
                    Console.Write("Digite a linha...");
                    choice_l = int.Parse(Console.ReadLine());
                } while (choice_l <= 0 || choice_l > SIZE);

                do
                {
                    Console.Write("Digite a coluna...");
                    choice_c = int.Parse(Console.ReadLine());
                } while (choice_c <= 0 || choice_c > SIZE);

                // Subtrair 1 da coordenada para acessar nos vetores
                choice_l -= 1;
                choice_c -= 1;

                // Se houver uma bomba
                if (board[choice_l, choice_c] == 1)
                {
                    finished = true;
                } else
                {
                    descobertos++;

                    // Fazer a checagem por bombas adjacentes
                    // Talvez dê pra fazer desde o início e guardar as informações?

                    int qtd_bombas = 0;

                    /*
                     *           choice_c-1 choice_c choice_c+1
                     * choice_l-1   0           1       2
                     * choice_l     3           X       4
                     * choice_l+1   5           6       7           
                     *
                     */

                    for(int l = choice_l - 1; l <= choice_l+1; ++l)
                    {
                        // Se a linha estiver fora do vetor, pular
                        if (l < 0 || l >= SIZE) continue;

                        for (int c = choice_c - 1; c <= choice_c + 1; ++c)
                        {
                            // Se a coluna estiver fora do vetor, pular
                            if (c < 0 || c >= SIZE) continue;

                            // Pular se for mesma coordenada
                            if (choice_c == c && choice_l == l) continue;

                            // Se houver bomba
                            if (board[l, c] == 1)
                            {
                                qtd_bombas++;
                            }
                        }
                    }
                    
                    // Se não houver bombas adjacentes
                    if (qtd_bombas == 0)
                    {
                        display[choice_l, choice_c] = ' ';
                    } else
                    {
                        // Transformar o número em caractere
                        // (Converter para string e pegar o primeiro caractere)
                        display[choice_l, choice_c] = qtd_bombas.ToString()[0];
                    }

                    // Se achou todos os espaços que não sejam minas
                    if (descobertos == SIZE*SIZE - QTD_MINAS) {
                        won = true;
                        finished = true;
                    }
                }
            }

            // Mensagem final (GAME OVER)
            if (won)
            {
                Console.WriteLine("Parabéns! Você venceu!");
            } else
            {
                Console.WriteLine("Você pisou numa bomba! Não foi dessa vez! :(");
            }

            // Exibir mapa
            Console.WriteLine("Mapa de onde havia bombas: (0 = Vazio ; 1 = Bomba)");

            for (int l = 0; l < SIZE; ++l)
            {
                for (int c = 0; c < SIZE; ++c)
                {
                    Console.Write("{0} ", board[l, c]);
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
