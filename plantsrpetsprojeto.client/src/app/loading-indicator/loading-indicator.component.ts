import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-indicator',
  standalone: false,
  
  templateUrl: './loading-indicator.component.html',
  styleUrl: './loading-indicator.component.css'
})
export class LoadingIndicatorComponent {
  @Input() isLoading: boolean = false;
}
