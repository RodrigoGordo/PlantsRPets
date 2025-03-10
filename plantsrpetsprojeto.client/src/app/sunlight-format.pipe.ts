import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sunlightFormat',
  standalone: false
})
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
