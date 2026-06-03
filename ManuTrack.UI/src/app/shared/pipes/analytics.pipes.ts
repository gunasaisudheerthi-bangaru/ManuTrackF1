import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'typeDesc', standalone: false, pure: true })
export class TypeDescPipe implements PipeTransform {
  transform(types: { value: string; label: string; desc: string }[], value: string): string {
    return types?.find(t => t.value === value)?.desc ?? '';
  }
}
