import { GroupComponent } from './groups/group/group.component';
import { SignupComponent } from './signup/signup.component';
import { HomeComponent } from './home/home.component';
import { RequestsComponent } from './requests/requests.component';
import { ProfileComponent } from './users/profile/profile.component';
import { UsersComponent } from './users/users.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'signup', component: SignupComponent },
  { path: 'home', component: HomeComponent },
  { path: 'users', component: UsersComponent },
  { path: 'groups/:name', component: GroupComponent },
  { path: 'users/:username', component: ProfileComponent },
  { path: 'users/:username/requests', component: RequestsComponent }
];

export const AppRoutes = RouterModule.forRoot(routes);
