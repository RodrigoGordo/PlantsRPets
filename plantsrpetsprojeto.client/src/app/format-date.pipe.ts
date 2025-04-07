import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatDate',
  standalone: false
})

/**
 * Pipe responsável por formatar uma data para o formato "DD-MM-YY HH:mm".
 *
 * Aceita valores do tipo string ou Date. Caso a data seja inválida ou nula,
 * devolve "N/A" ou "Invalid date".
 *
 * Exemplo de entrada: "2025-04-03T15:30:00Z"
 * Exemplo de saída: "03-04-25 15:30"
 */
export class FormatDatePipe implements PipeTransform {
  transform(value: string | Date | null | undefined): string {
    if (!value) return 'N/A';

    const date = new Date(value);
    if (isNaN(date.getTime())) return 'Invalid date';

    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear().toString().slice(-2);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');

    return `${day}-${month}-${year} ${hours}:${minutes}`;
  }
}
