import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sunlightFormat',
  standalone: false
})

/**
 * Pipe responsável por formatar uma lista de condições de luz solar.
 * Transforma cada termo numa string capitalizada e separa por vírgulas.
 *
 * Exemplo de entrada: ["full sun", "partial shade"]
 * Exemplo de saída: "Full Sun, Partial Shade"
 */
export class SunlightFormatPipe implements PipeTransform {
  transform(value: string[]): string {
    if (!value || !value.length) return '';
    return value
      .map(sun => sun.split(' ')
        .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase())
        .join(' ')
      )
      .join(', ');
  }
}
