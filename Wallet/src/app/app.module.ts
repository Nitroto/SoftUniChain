import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, PatternValidator, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule, MatCheckboxModule, MatDialogModule,
          MatExpansionModule, MatFormFieldModule, MatIconModule,
          MatSelectModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastyModule } from 'ng2-toasty';
import { NgxQRCodeModule} from '@techiediaries/ngx-qrcode';

import { AppComponent } from './components/app/app.component';
import { TransactionContentDialog, WalletComponent } from './components/wallet/wallet.component';
import { HomeComponent } from './components/home/home.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { WalletService } from './services/wallet.service';


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
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    NgxQRCodeModule,
    MatExpansionModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatSelectModule,
    MatDialogModule,
    MatCheckboxModule,
    MatFormFieldModule,
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
  providers: [
    WalletService,
    PatternValidator
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
