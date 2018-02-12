import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule, MatIconModule, MatSelectModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastyModule } from 'ng2-toasty';
import { NgxQRCodeModule} from '@techiediaries/ngx-qrcode';

import { AppComponent } from './components/app/app.component';
import { TransactionContentDialog, WalletComponent } from './components/wallet/wallet.component';
import { HomeComponent } from './components/home/home.component';
import { NavigationComponent } from './components/navigation/navigation.component';

@NgModule({
  declarations: [
    AppComponent,
    WalletComponent,
    HomeComponent,
    NavigationComponent,
    TransactionContentDialog
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    NgxQRCodeModule,
    MatInputModule,
    MatIconModule,
    MatSelectModule,
    MatDialogModule,
    NgbModule.forRoot(),
    ToastyModule.forRoot(),
    RouterModule.forRoot([
      { path: '', redirectTo: '/home' , pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'wallet', component: WalletComponent },
      { path: 'wallet/exit', component: WalletComponent },
      { path: '**', redirectTo: '/home'}
    ])
  ],
  entryComponents: [
    TransactionContentDialog
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
