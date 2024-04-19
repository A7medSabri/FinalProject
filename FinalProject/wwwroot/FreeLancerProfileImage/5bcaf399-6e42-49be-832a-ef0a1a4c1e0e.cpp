#include <bits/stdc++.h>

#define ll long long
#define ld long double
#define stp(n) fixed<<setprecision(n)
#define flash cin.tie(0); cin.sync_with_stdio(0);
#define el '\n'
#define Point pair<double, double>   // Define Point as a pair of doubles

using namespace std;

////
#pragma GCC optimize("03")
#pragma GCC target("tune=native")
#pragma GCC optimize("unroll-loops")


const int inf = 1e9, N = 2e3 + 9, mod = 998244353, M = 6e3;
ll dp[2][N];
vector<vector<pair<int, int>>> W(N);

void testCase() {
    int n, s;
    cin >> s >> n;
    for (int i = 0; i < n; ++i) {
        int vi, wi, ki;
        cin >> vi >> wi >> ki;
        W[wi].push_back({vi, ki});
    }
    for (int i = 1; i <= s; i++)sort(W[i].rbegin(), W[i].rend());
    int p = 0;
    memset(dp, 0, sizeof dp);
    for (int w = s; w >= 1; w--) {
        p = !p;
        for (int rem = 0; rem <= s; rem++) {
            ll &ans = dp[p][rem];
            ans = dp[!p][rem];
            ll sum = 0, cnt = 0, l = 0;
            for (int i = 1; i <= (rem / w); i++) {
                if (l == W[w].size())break;
                cnt++, sum += W[w][l].first;
                ans = max(ans, dp[!p][rem - i * w] + sum);
                if (cnt == W[w][l].second)l++, cnt = 0;
            }
        }
    }
    cout << dp[p][s];

}

int main() {

//    freopen("feast.in", "r", stdin);
//    freopen("feast.out", "w", stdout);

    flash;
    int t = 1;
    //cin >> t;
    while (t--) {
        testCase();
    }

}

