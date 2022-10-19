export interface Address {
  /** Full mailing address, formatted for display or use on a mailing label */
  formatted?: string;
  /** Full street address component, which MAY include house number, street name, Post Office Box, and multi-line extended street address information */
  street_address?: string;
  /** City or locality component */
  locality?: string;
  /** State, province, prefecture, or region component */
  region?: string;
  /** Zip code or postal code component */
  postal_code?: string;
  /** Country name component */
  country?: string;
}
