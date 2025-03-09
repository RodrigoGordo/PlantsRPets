import { Component, Input } from '@angular/core';
import { Tip } from '../models/tip-model';

@Component({
  selector: 'app-tip-cards',
  templateUrl: './tip-cards.component.html',
  styleUrls: ['./tip-cards.component.css']
})
export class TipCardsComponent {
  @Input() tips: Tip[] = [];
}
